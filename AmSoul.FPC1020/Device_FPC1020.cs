using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace AmSoul.FPC1020
{
    public enum DriveCode
    {
        DRIVE_UNKNOWN = 0,
        DRIVE_NO_ROOT_DIR = 1,
        DRIVE_REMOVABLE = 2,
        DRIVE_FIXED = 3,
        DRIVE_REMOTE = 4,
        DRIVE_CDROM = 5,
        DRIVE_RAMDISK = 6
    }
    public class Device_FPC1020
    {
        private readonly USBDriver usb = new();
        public bool Connect(string drive = null)
        {
            if (string.IsNullOrEmpty(drive))
            {
                var drives = Environment.GetLogicalDrives();
                foreach (string driveStr in drives)
                {
                    var driveType = Win32Native._GetDriveType(driveStr);
                    if (driveType != (int)DriveCode.DRIVE_REMOVABLE && driveType != (int)DriveCode.DRIVE_CDROM) continue;

                    print($"-------------------------------------------");
                    print($"Connecting device: {driveStr} , {driveType}");
                    if (!usb.Connect(driveStr)) continue;
                    if (!RunTestConnection()) continue;
                    return true;
                }
            }
            else
            {
                print($"Connecting device: {drive}");
                if (!usb.Connect(drive)) return false;
                if (!RunTestConnection()) return false;
                return true;
            }
            return false;
        }

        public void Disconnect() => usb.Disconnect();
        //public bool Execute(ScsiCommandCode code) => this.usb.Ioctl(this.Commands[code].Sptw);

        public CommandPacket InitCmdPacket(CommandCode cmdCode, byte[] data = null, byte srcDeviceID = 0, byte dstDeviceID = 0)
        {
            var packet = new CommandPacket()
            {
                Prefix = Convert.ToUInt16(PacketCode.CMD_PREFIX_CODE),
                SrcDeviceID = srcDeviceID,
                DstDeviceID = dstDeviceID,
                CMDCode = Convert.ToUInt16(cmdCode),
                Data = new byte[16],
                DataLen = (ushort)(data != null ? data.Length : 0)
            };
            if (data != null && data.Length > 0)
                Array.Copy(data, packet.Data, data.Length);

            var bytePacket = StructureHelper.StructToBytes(packet);
            ushort checkSum = 0;
            for (var i = 0; i < Marshal.SizeOf(packet) - 2; i++)
                checkSum += bytePacket[i];

            packet.CheckSum = checkSum;
            return packet;
        }
        public ErrorCode USBSendPacket(CommandPacket packet)
        {
            var bytePacket = StructureHelper.StructToBytes(packet);
            byte[] btCDB = new byte[8];
            btCDB[0] = 0xEF;
            btCDB[1] = 0x11;
            btCDB[4] = (byte)bytePacket.Length;

            SCSIPassThroughDirectWrapper sptdw = new(btCDB, bytePacket, DataDirection.SCSI_IOCTL_DATA_OUT, (uint)bytePacket.Length);
            print($"Send Packet: {StructureHelper.byteToHexStr(sptdw.GetDataBuffer(), 26)}");
            if (!usb.Ioctl(sptdw)) return ErrorCode.FAIL;
            print($"Send Packet Success!");

            return USBReceiveAck(packet);
        }
        public ErrorCode USBSendDataPacket(CommandPacket packet)
        {
            var bytePacket = StructureHelper.StructToBytes(packet);
            byte[] btCDB = new byte[8];
            btCDB[0] = 0xEF;
            btCDB[1] = 0x13;
            btCDB[4] = (byte)bytePacket.Length;

            SCSIPassThroughDirectWrapper sptdw = new(btCDB, bytePacket, DataDirection.SCSI_IOCTL_DATA_OUT, (uint)bytePacket.Length);
            print($"Send Data Packet: {StructureHelper.byteToHexStr(sptdw.GetDataBuffer(), 26)}");
            if (!usb.Ioctl(sptdw)) return ErrorCode.FAIL;
            print($"Send Data Packet Success!");

            return USBReceiveDataAck(packet);
        }

        public ErrorCode USBReceiveAck(CommandPacket packet)
        {
            var cmdPacket = StructureHelper.StructToBytes(packet);

            byte[] btCDB = new byte[8];
            btCDB[0] = 0xEF;
            btCDB[1] = 0x12;

            byte[] receivedPacket;
            byte[] waitPacket = new byte[65536];
            StructureHelper.MemSet(waitPacket, 0xAF, cmdPacket.Length);
            ushort timeOut = 5;

            if (packet.CMDCode == (ushort)CommandCode.CMD_TEST_CONNECTION)
                timeOut = 3;
            if (packet.CMDCode == (ushort)CommandCode.CMD_ADJUST_SENSOR)
                timeOut = 30;

            int readCount = GetReadWaitTime(packet.CMDCode);
            int count = 0;
            SCSIPassThroughDirectWrapper sptw = new(btCDB, cmdPacket, DataDirection.SCSI_IOCTL_DATA_IN, (uint)cmdPacket.Length, timeOut);
            do
            {
                print($"Wait Response Packet...");
                if (!usb.Ioctl(sptw)) return ErrorCode.FAIL;

                Thread.Sleep(40);

                count++;
                if (count > readCount)
                    return ErrorCode.FAIL;
                receivedPacket = sptw.GetDataBuffer();
            }
            while (Win32Native.ByteArrayCompare(receivedPacket, waitPacket));

            print($"Response Packet Success:{StructureHelper.byteToHexStr(receivedPacket, 26)}");

            return CheckReceive(packet, receivedPacket);
        }
        public ErrorCode USBReceiveDataAck(CommandPacket packet)
        {
            var cmdPacket = StructureHelper.StructToBytes(packet);

            byte[] btCDB = new byte[8];
            btCDB[0] = 0xEF;
            btCDB[1] = 0x15;

            byte[] receivedPacket;
            byte[] waitPacket = new byte[10];
            StructureHelper.MemSet(waitPacket, 0xAF, 10);
            ushort timeOut = 5000;

            SCSIPassThroughDirectWrapper sptw = new(btCDB, cmdPacket, DataDirection.SCSI_IOCTL_DATA_IN, 8, timeOut);
            do
            {
                if (!usb.Ioctl(sptw)) return ErrorCode.FAIL;
                receivedPacket = sptw.GetDataBuffer();
                Thread.Sleep(40);

            }
            while (Win32Native.ByteArrayCompare(receivedPacket, waitPacket));

            print($"Response Data Packet Success:{StructureHelper.byteToHexStr(receivedPacket, 26)}");

            if (!ReceiveRawData(receivedPacket)) return ErrorCode.FAIL;
            //receieveRawData
            return CheckReceive(packet, receivedPacket);
        }
        public bool ReceiveRawData(byte[] buffer)
        {
            byte[] btCDB = new byte[8];
            btCDB[0] = 0xEF;
            btCDB[1] = 0x14;

            SCSIPassThroughDirectWrapper sptw = new(btCDB, buffer, DataDirection.SCSI_IOCTL_DATA_IN, (uint)buffer.Length, 5);
            if (!usb.Ioctl(sptw)) return false;
            var raw = sptw.GetDataBuffer();
            print($"Response RawData :{StructureHelper.byteToHexStr(raw, 48)}");
            return true;
        }
        public ErrorCode CheckReceive(CommandPacket cmdPacket, byte[] receivedPacket)
        {
            print($"Check Response Packet...");

            var rcmPacket = (ResponsePacket)StructureHelper.ByteToStruct(receivedPacket, typeof(ResponsePacket));
            if (rcmPacket.Prefix != (ushort)PacketCode.RCM_PREFIX_CODE) return ErrorCode.ERR_PREFIX_CODE;

            int calcCheckSum = 0;
            for (var i = 0; i < Marshal.SizeOf(cmdPacket) - 2; i++)
                calcCheckSum += receivedPacket[i];
            //Check checkSum
            if (rcmPacket.CheckSum != calcCheckSum) return ErrorCode.ERR_CHECKSUM;
            //Check CMDCode
            if (cmdPacket.CMDCode != rcmPacket.ResponseCmdCode) return ErrorCode.FAIL;
            if (rcmPacket.ResultCode == 0)
                print("Check Success!");
            else
                print("Check Fail");
            return rcmPacket.ResultCode == 0
                ? ErrorCode.SUCCESS
                : ErrorCode.FAIL;
        }
        public ushort GetReadWaitTime(ushort cmdCode)
        {
            ushort time;
            switch (cmdCode)
            {
                case (ushort)CommandCode.CMD_ADJUST_SENSOR:
                    time = 0xFFFF;
                    break;
                default:
                    time = 150;
                    break;
            }
            return time;
        }
        public void print(string content)
        {
            Console.WriteLine(content);
        }
        public bool RunTestConnection()
        {
            var packet = InitCmdPacket(CommandCode.CMD_TEST_CONNECTION);
            var result = USBSendPacket(packet);
            if (result != ErrorCode.SUCCESS)
            {
                print($"Connecting Fail! {result}");
                return false;
            }
            else
            {
                print("Connecting Success!");
                return true;
            }
        }
        public void RunSetParam(ParamCode code, byte value)
        {
            var param = new byte[] { (byte)code, value };
            var packet = InitCmdPacket(CommandCode.CMD_SET_PARAM, param);
            var result = USBSendPacket(packet);
            if (result != ErrorCode.SUCCESS)
                print($"{result}");
            else
                print("Set Param Success!");
        }
        public void RunGetParam(ParamCode code)
        {
            var param = new byte[] { (byte)code };
            var packet = InitCmdPacket(CommandCode.CMD_GET_PARAM, param);
            var result = USBSendPacket(packet);
            if (result != ErrorCode.SUCCESS)
                print($"{result}");
            else
                print("Get Param Success!");
        }
        public void RunGetDeviceInfo()
        {
            var packet = InitCmdPacket(CommandCode.CMD_GET_DEVICE_INFO);
            var result = USBSendPacket(packet);
            if (result != ErrorCode.SUCCESS)
                print($"{result}");
            else
            {
                USBReceiveDataAck(packet);
                print("Get DeviceInfo Success!");
            }
        }
        public void RunFingerDetect()
        {
            var packet = InitCmdPacket(CommandCode.CMD_FINGER_DETECT);

            do
            {
                print($"Put Finger !");
                Thread.Sleep(2000);
            } while (USBSendPacket(packet) != ErrorCode.SUCCESS);
            print("Finger Detect!");
            //return true;

        }
    }
}
