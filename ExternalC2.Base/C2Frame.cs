using System;
using System.Runtime.InteropServices;
using System.Text;

namespace ExternalC2.Base
{
    /// <summary>
    /// All	frames start with a 4-byte little-endian byte order integer. This integer is the length of the data within
    /// the	frame. The frame data always follows this length value.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct C2Frame
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
    }
}