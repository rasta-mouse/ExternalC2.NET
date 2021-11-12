using System;
using System.Security.Cryptography;

using ExternalC2.NET.Base;

using Xunit;

namespace ExternalC2.NET.UnitTests
{
    public class BaseTests
    {
        [Fact]
        public void ConvertFrameToByteArray()
        {
            var length =  BitConverter.GetBytes(20);
            var data = new byte[20];
            
            using var rng = RandomNumberGenerator.Create();
            rng.GetNonZeroBytes(data);

            var originalFrame = new C2Frame(length, data);
            var frameBytes = originalFrame.ToByteArray();
            var newFrame = C2Frame.FromByteArray(frameBytes);
            
            Assert.Equal(newFrame.Length, originalFrame.Length);
            Assert.Equal(newFrame.Data, originalFrame.Data);
        }

        [Fact]
        public void ConvertFrameToBase64String()
        {
            var length =  BitConverter.GetBytes(20);
            var data = new byte[20];
            
            using var rng = RandomNumberGenerator.Create();
            rng.GetNonZeroBytes(data);

            var originalFrame = new C2Frame(length, data);
            var frameString = originalFrame.ToBase64String();
            var newFrame = C2Frame.FromBase64String(frameString);
            
            Assert.Equal(newFrame.Length, originalFrame.Length);
            Assert.Equal(newFrame.Data, originalFrame.Data);
        }
    }
}