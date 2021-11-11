using System;
using System.IO;
using System.Threading.Tasks;

namespace ExternalC2.Base
{
    public abstract class BaseConnector
    {
        protected abstract Stream Stream { get; set; }

        protected async Task<C2Frame> ReadFrame()
        {
            // read first 4 bytes
            // this is data length
            var lengthBuf = new byte[4];
            var read = await Stream.ReadAsync(lengthBuf, 0, 4);

            if (read != lengthBuf.Length)
                throw new Exception("Failed to read frame length");

            var expectedLength = BitConverter.ToInt32(lengthBuf, 0);

            // now read that length
            // the previous read consumes the 4 bytes from the stream
            // had issues reading in a single chunk
            // so read in smaller increments
            
            var totalRead = 0;
            using var ms = new MemoryStream();

            do
            {
                var buf = new byte[1024];
                read = await Stream.ReadAsync(buf, 0, 1024);
                await ms.WriteAsync(buf, 0, read);
                totalRead += read;
                
            } while (totalRead < expectedLength);

            return new C2Frame(lengthBuf, ms.ToArray());
        }

        protected async Task WriteFrame(C2Frame frame)
        {
            await Stream.WriteAsync(frame.Length, 0, frame.Length.Length);
            await Stream.WriteAsync(frame.Data, 0, frame.Data.Length);
        }
    }
}