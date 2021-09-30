using System;
using System.IO;
using System.Runtime.InteropServices;

namespace AmSoul.FPC1020
{
    public class USBDriver
    {
        public enum IoctlCodes
        {
            IOCTL_SCSI_PASS_THROUGH_DIRECT = 0x0004D004,
            IOCTL_STORAGE_EJECT_MEDIA = 0x2D4808
        }

        private const uint GENERIC_READ = 0x80000000;
        private const uint GENERIC_WRITE = 0x40000000;
        private const uint FILE_SHARE_READ = 0x00000001;
        private const uint FILE_SHARE_WRITE = 0x00000002;

        private const byte CDB6GENERIC_LENGTH = 6;
        private const byte CDB10GENERIC_LENGTH = 10;

        //private const uint METHOD_BUFFERED = 0;
        //private const uint IOCTL_SCSI_BASE = 0x00000004;
        //private const uint FILE_READ_ACCESS = 0x0001;
        //private const uint FILE_WRITE_ACCESS = 0x0002;
        //private const uint IOCTL_SCSI_PASS_THROUGH = ((IOCTL_SCSI_BASE) << 16) | ((FILE_READ_ACCESS | FILE_WRITE_ACCESS) << 14) | (0x0401 << 2) | (METHOD_BUFFERED);
        //private const uint IOCTL_SCSI_PASS_THROUGH_DIRECT = ((IOCTL_SCSI_BASE) << 16) | ((FILE_READ_ACCESS | FILE_WRITE_ACCESS) << 14) | (0x0405 << 2) | (METHOD_BUFFERED);

        private IntPtr handle = IntPtr.Zero;
        private uint lastError = 0;

        public uint GetError() => lastError;

        public bool Ioctl(SCSIPassThroughDirectWrapper sptdw)
        {
            IntPtr bufferPointer = IntPtr.Zero;
            bool ioresult = false;

            try
            {
                int bufferSize = Marshal.SizeOf(sptdw.sptdwb);
                bufferPointer = Marshal.AllocHGlobal(bufferSize);
                Marshal.StructureToPtr(sptdw.sptdwb, bufferPointer, true);

                ioresult = Win32Native._DeviceIoControl(handle,
                    (uint)IoctlCodes.IOCTL_SCSI_PASS_THROUGH_DIRECT,
                    bufferPointer,
                    (uint)bufferSize,
                    bufferPointer,
                    (uint)bufferSize,
                    out uint bytesReturned,
                    IntPtr.Zero) && (bytesReturned > 0);

                if (ioresult)
                {
                    sptdw.sptdwb = (ScsiPassThroughDirectWithBuffers)Marshal.PtrToStructure(bufferPointer, typeof(ScsiPassThroughDirectWithBuffers));
                }
                else
                {
                    lastError = Win32Native._GetLastError();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("IO Ctl FAIL: {0}", e.Message);
            }
            finally
            {
                Marshal.FreeHGlobal(bufferPointer);
            }

            return ioresult;
        }
        public bool Connect(string driveName)
        {
            string path = @"\\.\" + driveName;
            if (path.Length - path.LastIndexOf(':') > 1)
                path = path.Remove(path.LastIndexOf(':') + 1);

            uint desiredAccess = GENERIC_READ | GENERIC_WRITE;
            uint shareMode = FILE_SHARE_READ | FILE_SHARE_WRITE;
            handle = Win32Native._CreateFileW(path,
                desiredAccess,
                shareMode,
                IntPtr.Zero,
                (uint)FileMode.Open,
                0,//(uint)FileAttributes.Normal, 
                IntPtr.Zero);
            return (long)handle != -1;
        }
        public void Disconnect() => Win32Native._CloseHandle(handle);
    }
}
