using System;
using System.Net;
using System.Threading.Tasks;

using ExternalC2.Controller;

using Xunit;

namespace ExternalC2.UnitTests
{
    public class BeaconControllerTests
    {
        [Fact]
        public void ConfigureController()
        {
            var controller = new BeaconController();
            controller.Configure(IPAddress.Parse(Constants.Server), Constants.Port, Constants.Block);
            
            Assert.Equal(IPAddress.Parse(Constants.Server), controller.Server);
            Assert.Equal(Constants.Port, controller.Port);
            Assert.Equal(Constants.Block, controller.Block);
        }
        
        [Fact]
        public async Task ConnectToController()
        {
            var controller = new BeaconController();
            controller.Configure(IPAddress.Parse(Constants.Server), Constants.Port, Constants.Block);
            
            var connected = await controller.Connect();
            
            Assert.True(connected);
        }

        [Fact]
        public async Task RequestX64Stage()
        {
            var pipeName = Guid.NewGuid().ToString();

            var controller = new BeaconController();
            controller.Configure(IPAddress.Parse(Constants.Server), Constants.Port, Constants.Block);
            await controller.Connect();

            var stage = await controller.RequestStage(pipeName, Base.Architecture.x64);
            
            Assert.NotNull(stage);
            Assert.True(stage.Length == 253440);
        }
        
        [Fact]
        public async Task RequestX86Stage()
        {
            var pipeName = Guid.NewGuid().ToString();

            var controller = new BeaconController();
            controller.Configure(IPAddress.Parse(Constants.Server), Constants.Port, Constants.Block);
            await controller.Connect();

            var stage = await controller.RequestStage(pipeName, Base.Architecture.x86);
            
            Assert.NotNull(stage);
            Assert.True(stage.Length == 203776);
        }
    }
}