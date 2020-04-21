using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace FilePropertiesDataObject
{
    public static class Win32Helper
    {
        public const uint FILE_READ_ATTRIBUTES = (0x0080);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern SafeFileHandle CreateFile(
           string lpFileName,
           [MarshalAs(UnmanagedType.U4)] FileAccess dwDesiredAccess,
           [MarshalAs(UnmanagedType.U4)] FileShare dwShareMode,
           IntPtr lpSecurityAttributes,
           [MarshalAs(UnmanagedType.U4)] FileMode dwCreationDisposition,
           [MarshalAs(UnmanagedType.U4)] FileAttributes dwFlagsAndAttributes,
           IntPtr hTemplateFile);

    }
}
