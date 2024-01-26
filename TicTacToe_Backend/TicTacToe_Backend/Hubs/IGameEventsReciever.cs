using Domain.TicTacToe.GameEvents;

namespace TicacToe_Backend.Hubs
{
    public interface IGameEventsReciever
    {
        public Task GameEvent(TicTacToeGameEvent @event);

        public Task RoomMessage(string username, string message);
    }
}
