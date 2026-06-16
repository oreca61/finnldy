// prompt: Schreibe einen xUnit Test für SendSwipeOverNetwork, wenn keine Verbindung besteht

using Finnldy.BLL;
using Finnldy.DAL;
using Xunit;

namespace Finnldy.Tests;

public class LobbyControllerNetworkTests
{
    [Fact]
    public async Task SendSwipeOverNetwork_WhenNotConnected_RaisesStatusMessage()
    {
        // Arrange
        var controller = new LobbyController();

        string? receivedMessage = null;

        controller.NetworkStatusChanged += message =>
        {
            receivedMessage = message;
        };

        bool oldIsHost = NetworkSession.IsHost;
        bool oldIsClient = NetworkSession.IsClient;

        try
        {
            NetworkSession.IsHost = false;
            NetworkSession.IsClient = false;

            var movie = TestDataFactory.CreateMovie(1, "Test Film");

            // Act
            await controller.SendSwipeOverNetwork(movie, SwipeType.Like);

            // Assert
            Assert.Equal("Nicht verbunden. Swipe wurde nicht gesendet.", receivedMessage);
        }
        finally
        {
            NetworkSession.IsHost = oldIsHost;
            NetworkSession.IsClient = oldIsClient;
        }
    }
}