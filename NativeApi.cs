using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace USBWatcher
{
    internal static class NativeApi
    {
        public const uint SystemHandleInformation = 16;
        public const uint ObjectTypeFile = 28;

        [StructLayout(LayoutKind.Sequential)]
        public struct SYSTEM_HANDLE_TABLE_ENTRY_INFO
        {
            public ushort UniqueProcessId;
            public ushort CreatorBackTraceIndex;
            public byte ObjectTypeIndex;
            public byte HandleAttributes;
            public ushort HandleValue;
            public IntPtr Object;
            public IntPtr AccessMask;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SYSTEM_HANDLE_INFORMATION
        {
            public uint NumberOfHandles;
            public IntPtr Handles;
        }

        public enum OBJECT_INFORMATION_CLASS
        {
            ObjectBasicInformation = 0,
            ObjectNameInformation = 1,
            ObjectTypeInformation = 2,
            ObjectAllTypesInformation = 3,
            ObjectHandleInformation = 4
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct UNICODE_STRING
        {
            public ushort Length;
            public ushort MaximumLength;
            public IntPtr Buffer;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct OBJECT_NAME_INFORMATION
        {
            public UNICODE_STRING Name;
            UNICODE_STRING _Padding => default;
        }

        [DllImport("ntdll.dll", SetLastError = true)]
        public static extern uint NtQuerySystemInformation(
            uint SystemInformationClass,
            IntPtr SystemInformation,
            uint SystemInformationLength,
            out uint ReturnLength);

        [DllImport("ntdll.dll", SetLastError = true)]
        public static extern uint NtQueryObject(
            IntPtr Handle,
            OBJECT_INFORMATION_CLASS ObjectInformationClass,
            IntPtr ObjectInformation,
            uint ObjectInformationLength,
            out uint ReturnLength);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool DuplicateHandle(
            IntPtr hSourceProcessHandle,
            IntPtr hSourceHandle,
            IntPtr hTargetProcessHandle,
            out IntPtr lpTargetHandle,
            uint dwDesiredAccess,
            bool bInheritHandle,
            uint dwOptions);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetCurrentProcess();

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern uint QueryDosDevice(string lpDeviceName, StringBuilder lpTargetPath, uint ucchMax);

        public const uint PROCESS_DUP_HANDLE = 0x0040;
        public const uint DUPLICATE_CLOSE_SOURCE = 0x00000001;
        public const uint DUPLICATE_SAME_ACCESS = 0x00000002;

        public static string? GetHandleName(IntPtr handle, IntPtr processHandle)
        {
            IntPtr dupHandle = IntPtr.Zero;
            try
            {
                if (!DuplicateHandle(processHandle, handle, GetCurrentProcess(),
                    out dupHandle, 0, false, DUPLICATE_SAME_ACCESS))
                {
                    return null;
                }

                uint returnLength = 0;
                uint status = NtQueryObject(dupHandle, OBJECT_INFORMATION_CLASS.ObjectNameInformation,
                    IntPtr.Zero, 0, out returnLength);

                if (returnLength == 0) return null;

                IntPtr buffer = Marshal.AllocHGlobal((int)returnLength);
                try
                {
                    status = NtQueryObject(dupHandle, OBJECT_INFORMATION_CLASS.ObjectNameInformation,
                        buffer, returnLength, out returnLength);

                    if (status != 0) return null;

                    var nameInfo = Marshal.PtrToStructure<OBJECT_NAME_INFORMATION>(buffer);
                    if (nameInfo.Name.Buffer == IntPtr.Zero) return null;

                    return Marshal.PtrToStringUni(nameInfo.Name.Buffer, nameInfo.Name.Length / 2);
                }
                finally
                {
                    Marshal.FreeHGlobal(buffer);
                }
            }
            finally
            {
                if (dupHandle != IntPtr.Zero)
                    CloseHandle(dupHandle);
            }
        }

        private static Dictionary<string, char> _deviceToDrive;
        private static object _lock = new object();

        public static void BuildDeviceToDriveMap()
        {
            lock (_lock)
            {
                _deviceToDrive = new Dictionary<string, char>(StringComparer.OrdinalIgnoreCase);

                for (char c = 'A'; c <= 'Z'; c++)
                {
                    string driveName = $"{c}:";
                    StringBuilder sb = new StringBuilder(256);
                    uint result = QueryDosDevice(driveName, sb, (uint)sb.Capacity);
                    if (result > 0)
                    {
                        string devicePath = sb.ToString();
                        _deviceToDrive[devicePath] = c;
                    }
                }
            }
        }

        public static char? GetDriveLetterFromDevicePath(string devicePath)
        {
            if (_deviceToDrive == null) return null;

            foreach (var kvp in _deviceToDrive)
            {
                if (devicePath.StartsWith(kvp.Key, StringComparison.OrdinalIgnoreCase))
                {
                    return kvp.Value;
                }
            }
            return null;
        }
    }
}
