using System;

namespace USBWatcher
{
    public class ProcessHandleInfo
    {
        public int ProcessId { get; set; }
        public string ProcessName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public IntPtr Handle { get; set; }

        public override string ToString()
        {
            return $"{ProcessName} (PID: {ProcessId}) - {FilePath}";
        }
    }
}
