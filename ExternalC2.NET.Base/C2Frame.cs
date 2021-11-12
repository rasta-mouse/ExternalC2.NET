using System;
using System.Runtime.InteropServices;
using System.Text;

namespace ExternalC2.NET.Base
{
    /// <summary>
    /// All	frames start with a 4-byte little-endian byte order integer. This integer is the length of the data within
    /// the	frame. The frame data always follows this length value.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct C2Frame
    {
        public byte[] Length { get; }
        public byte[] Data { get; }

        public C2Frame(byte[] length, byte[] data)
        {
            Length = length;
            Data = data;
        }

        public static C2Frame Generate(string key, string value = "")
        {
            var bytes = Encoding.UTF8.GetBytes(!string.IsNullOrWhiteSpace(value)
                ? $"{key}={value}"
                : key);

            var length = BitConverter.GetBytes(bytes.Length);
            
            return new C2Frame(length, bytes);
        }
        
        public byte[] ToByteArray()
        {
            var buf = new byte[Length.Length + Data.Length];
            Buffer.BlockCopy(Length, 0, buf, 0, Length.Length);
            Buffer.BlockCopy(Data, 0, buf, Length.Length, Data.Length);

            return buf;
        }

        public static C2Frame FromByteArray(byte[] frame)
        {
            var dataLength = frame.Length - 4;
            
            var length = new byte[4];
            var data = new byte[dataLength];
            
            Buffer.BlockCopy(frame, 0, length, 0, 4);
            Buffer.BlockCopy(frame, 4, data, 0, dataLength);

            return new C2Frame(length, data);
        }

        public static C2Frame FromBase64String(string frame)
        {
            return FromByteArray(Convert.FromBase64String(frame));
        }

        public string ToBase64String()
        {
            return Convert.ToBase64String(ToByteArray());
        }
    }
}