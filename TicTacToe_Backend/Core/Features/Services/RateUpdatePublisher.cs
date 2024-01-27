using Contracts;
using Domain.TicTacToe;
using MassTransit;

namespace Features.Services
{
    public class RateUpdatePublisher : IAwarder
    {
        private readonly IBus _bus;

        public RateUpdatePublisher(IBus bus)
        {
            _bus = bus;
        }

        public Task ChangeUserRateAsync(string userId, int rateDelta)
        {
            return _bus.Publish(new UserRateChangeRequest() { UserId = userId, ChangeDelta = rateDelta });
        }
    }
}
