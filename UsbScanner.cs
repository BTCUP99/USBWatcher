using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace USBWatcher
{
    internal class UsbScanner
    {
        public List<DriveInfo> GetRemovableDrives()
        {
            var drives = new List<DriveInfo>();
            foreach (var drive in DriveInfo.GetDrives())
            {
                if (drive.DriveType == DriveType.Removable && drive.IsReady)
                {
                    drives.Add(drive);
                }
            }
            return drives;
        }

        public List<ProcessHandleInfo> GetProcessesUsingDrive(char driveLetter)
        {
            var results = new List<ProcessHandleInfo>();
            string drivePrefix = $"{driveLetter}:\\";

            NativeApi.BuildDeviceToDriveMap();

            uint returnLength = 0;
            uint status = NativeApi.NtQuerySystemInformation(
                NativeApi.SystemHandleInformation,
                IntPtr.Zero, 0, out returnLength);

            if (returnLength == 0) return results;

            IntPtr buffer = Marshal.AllocHGlobal((int)returnLength);
            try
            {
                status = NativeApi.NtQuerySystemInformation(
                    NativeApi.SystemHandleInformation,
                    buffer, returnLength, out returnLength);

                if (status != 0) return results;

                int handleCount = Marshal.ReadInt32(buffer);
                IntPtr handleEntryPtr = buffer + 4;

                var processedProcesses = new Dictionary<int, string>();

                for (int i = 0; i < handleCount; i++)
                {
                    var handleInfo = Marshal.PtrToStructure<NativeApi.SYSTEM_HANDLE_TABLE_ENTRY_INFO>(handleEntryPtr);

                    if (handleInfo.ObjectTypeIndex != NativeApi.ObjectTypeFile)
                    {
                        handleEntryPtr += Marshal.SizeOf<NativeApi.SYSTEM_HANDLE_TABLE_ENTRY_INFO>();
                        continue;
                    }

                    IntPtr processHandle = IntPtr.Zero;
                    try
                    {
                        processHandle = NativeApi.OpenProcess(
                            NativeApi.PROCESS_DUP_HANDLE | 0x0400, // PROCESS_QUERY_INFORMATION
                            false, handleInfo.UniqueProcessId);

                        if (processHandle == IntPtr.Zero)
                        {
                            handleEntryPtr += Marshal.SizeOf<NativeApi.SYSTEM_HANDLE_TABLE_ENTRY_INFO>();
                            continue;
                        }

                        IntPtr handleValue = new IntPtr(handleInfo.HandleValue);
                        string? filePath = NativeApi.GetHandleName(handleValue, processHandle);

                        if (!string.IsNullOrEmpty(filePath))
                        {
                            char? handleDrive = null;

                            // 尝试直接用 DOS 路径匹配（如 D:\...）
                            if (filePath.StartsWith(drivePrefix, StringComparison.OrdinalIgnoreCase))
                            {
                                handleDrive = driveLetter;
                            }
                            // 尝试用对象管理器路径匹配（如 \Device\HarddiskVolume2\...）
                            else
                            {
                                handleDrive = NativeApi.GetDriveLetterFromDevicePath(filePath);
                            }

                            if (handleDrive == driveLetter)
                            {
                                if (!processedProcesses.TryGetValue(handleInfo.UniqueProcessId, out string? processName))
                                {
                                    try
                                    {
                                        var process = Process.GetProcessById(handleInfo.UniqueProcessId);
                                        processName = process.ProcessName;
                                        processedProcesses[handleInfo.UniqueProcessId] = processName;
                                    }
                                    catch
                                    {
                                        processName = $"PID: {handleInfo.UniqueProcessId}";
                                        processedProcesses[handleInfo.UniqueProcessId] = processName;
                                    }
                                }

                                results.Add(new ProcessHandleInfo
                                {
                                    ProcessId = handleInfo.UniqueProcessId,
                                    ProcessName = processName,
                                    FilePath = filePath,
                                    Handle = handleValue
                                });
                            }
                        }
                    }
                    catch
                    {
                        // Skip processes we can't access
                    }
                    finally
                    {
                        if (processHandle != IntPtr.Zero)
                            NativeApi.CloseHandle(processHandle);
                    }

                    handleEntryPtr += Marshal.SizeOf<NativeApi.SYSTEM_HANDLE_TABLE_ENTRY_INFO>();
                }
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }

            return results.DistinctBy(x => new { x.ProcessId, x.FilePath }).ToList();
        }

        public bool KillProcess(int pid)
        {
            try
            {
                var process = Process.GetProcessById(pid);
                process.Kill();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    internal static class EnumerableExtensions
    {
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector)
        {
            var seenKeys = new HashSet<TKey>();
            foreach (var item in source)
            {
                if (seenKeys.Add(keySelector(item)))
                    yield return item;
            }
        }
    }
}
