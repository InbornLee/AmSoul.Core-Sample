using System;
using System.Runtime.InteropServices;

namespace AmSoul.FPC1020
{
    public static class Win32Native
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        private static extern int GetDriveType(string driveinfo);

        //        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        //        private static extern IntPtr CreateFileA(string lpFileName, uint dwDesiredAccess, uint dwShareMode, IntPtr SecurityAttributes,
        //                                                uint dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern IntPtr CreateFileW(string lpFileName, uint dwDesiredAccess, uint dwShareMode, IntPtr SecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);

        [DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool DeviceIoControl(IntPtr hDevice, uint dwIoControlCode, IntPtr lpInBuffer, uint nInBufferSize, IntPtr lpOutBuffer, uint nOutBufferSize, out uint lpBytesReturned, IntPtr lpOverlapped);

        [DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool DeviceIoControl(IntPtr hDevice, uint dwIoControlCode, byte[] lpInBuffer, uint nInBufferSize, IntPtr lpOutBuffer, uint nOutBufferSize, out uint lpBytesReturned, IntPtr lpOverlapped);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll")]
        private static extern uint GetLastError();

        [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern int memcmp(byte[] b1, byte[] b2, long count);

        [DllImport("ntdll.dll")]
        private static extern int RtlCompareMemory(IntPtr Destination, IntPtr Source, int Length);

        public static int _GetDriveType(string driveInfo) => GetDriveType(driveInfo);
        public static IntPtr _CreateFileW(string fileName, uint desiredAccess, uint shareMode, IntPtr securityAttributes, uint creationDisposition, uint flagsAndAttributes, IntPtr templateFile)
            => CreateFileW(fileName, desiredAccess, shareMode, securityAttributes, creationDisposition, flagsAndAttributes, templateFile);
        public static bool _DeviceIoControl(IntPtr device, uint ioControlCode, IntPtr inBuffer, uint inBufferSize, IntPtr outBuffer, uint outBufferSize, out uint bytesReturned, IntPtr overlapped)
            => DeviceIoControl(device, ioControlCode, inBuffer, inBufferSize, outBuffer, outBufferSize, out bytesReturned, overlapped);
        public static bool _DeviceIoControl(IntPtr device, uint ioControlCode, byte[] inBuffer, uint inBufferSize, IntPtr outBuffer, uint outBufferSize, out uint bytesReturned, IntPtr overlapped)
            => DeviceIoControl(device, ioControlCode, inBuffer, inBufferSize, outBuffer, outBufferSize, out bytesReturned, overlapped);
        public static bool _CloseHandle(IntPtr obj) => CloseHandle(obj);
        public static uint _GetLastError() => GetLastError();
        public static bool ByteArrayCompare(byte[] b1, byte[] b2)
        {
            return b1.Length == b2.Length && memcmp(b1, b2, b1.Length) == 0;
        }
        public static int rtlCompareMemory(IntPtr destination, IntPtr source, int length) => RtlCompareMemory(destination, source, length);
    }
}
