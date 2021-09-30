using System;
using System.Runtime.InteropServices;

namespace AmSoul.FPC1020
{
    public class StructureHelper
    {
        /// <summary>
        /// Structure To Bytes
        /// </summary>
        /// <param name="structObj"></param>
        /// <returns></returns>
        public static byte[] StructToBytes(object structObj)
        {
            int size = Marshal.SizeOf(structObj);
            IntPtr buffer = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.StructureToPtr(structObj, buffer, false);
                byte[] bytes = new byte[size];
                Marshal.Copy(buffer, bytes, 0, size);
                return bytes;
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }
        /// <summary>
        /// Byte to Structure
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object ByteToStruct(byte[] bytes, Type type)
        {
            int size = Marshal.SizeOf(type);
            if (size > bytes.Length)
            {
                return null;
            }
            //Allocate structure memory space
            IntPtr structPtr = Marshal.AllocHGlobal(size);
            //Copy the byte array to the allocated memory space
            Marshal.Copy(bytes, 0, structPtr, size);
            //Convert memory space to target structure
            object obj = Marshal.PtrToStructure(structPtr, type);
            //Free up memory space
            Marshal.FreeHGlobal(structPtr);
            return obj;
        }
        /// <summary>
        /// Bytes to IntPtr
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static IntPtr BytesToIntptr(byte[] bytes)
        {
            int size = bytes.Length;
            IntPtr buffer = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.Copy(bytes, 0, buffer, size);
                return buffer;
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }
        /// <summary> 
        /// Byte to Hex String 
        /// </summary> 
        /// <param name="bytes"></param> 
        /// <returns></returns> 
        public static string byteToHexStr(byte[] bytes, int length = 0)
        {
            string returnStr = "";
            int datalength;
            if (length == 0)
                datalength = bytes.Length;
            else
                datalength = length;

            if (bytes != null)
            {
                for (int i = 0; i < datalength; i++)
                {
                    returnStr += string.Format("{0:X2} ", bytes[i]);
                }
            }
            return returnStr;
        }
        /// <summary>
        /// Hex String to Bytes
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static byte[] StrToHexByte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }
        public static void MemSet(byte[] array, byte value, int setLen = 0)
        {
            if (array != null)
            {
                int block = 32, index = 0;
                int length = Math.Min(block, array.Length);
                if (setLen != 0)
                    length = setLen;

                //Fill the initial array
                while (index < length)
                {
                    array[index++] = value;
                }

                //length = array.Length;
                //while (index < length)
                //{
                //    Buffer.BlockCopy(array, 0, array, index, Math.Min(block, length - index));
                //    index += block;
                //    block *= 2;
                //}
            }
            else
            {
                throw new ArgumentNullException("array");
            }
        }
    }
}
