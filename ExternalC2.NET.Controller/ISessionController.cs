using System.Net;
using System.Threading.Tasks;

using ExternalC2.NET.Base;

namespace ExternalC2.NET.Controller
{
    public interface ISessionController
    {
        /// <summary>
        /// Configure the connection options for this controller
        /// </summary>
        /// <param name="serverAddress"></param>
        /// The IP address of the Cobalt Strike Team Server.
        /// <param name="serverPort"></param>
        /// The port of the External C2 listener.
        /// <param name="block"></param>
        /// A time in milliseconds that indicates how long the External C2 server should block when no new tasks are
        /// available. Once this time expires, the External C2 server will generate a no-op frame.
        void Configure(IPAddress serverAddress, int serverPort = 2222, int block = 100);

        /// <summary>
        /// Initialise the connection to the External C2 server.
        /// </summary>
        /// <returns></returns>
        Task<bool> Connect();

        /// <summary>
        /// The	architecture of the payload stage. Default's to x86.
        /// </summary>
        /// <param name="pipeName"></param>
        /// The named pipe name.
        /// <param name="arch"></param>
        /// The	architecture of the payload	stage.
        /// <returns>A byte array of the SMB Beacon stage.</returns>
        Task<byte[]> RequestStage(string pipeName, Architecture arch = Architecture.x86);

        /// <summary>
        /// Send a frame to the External C2 server.
        /// </summary>
        /// <param name="frame"></param>
        /// <returns></returns>
        Task WriteFrame(C2Frame frame);

        /// <summary>
        /// Read a frame from the External C2 server.
        /// </summary>
        /// <returns></returns>
        Task<C2Frame> ReadFrame();
    }
}