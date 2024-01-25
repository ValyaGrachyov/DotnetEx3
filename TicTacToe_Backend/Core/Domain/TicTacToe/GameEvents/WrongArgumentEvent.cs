

namespace Domain.TicTacToe.GameEvents
{
    public class WrongArgumentEvent : TicTacToeGameEvent
    {

        public string ApplicantId { get; init; }

        public TicTacToeRoomState GameState { get; init; }

        public int Row { get; init; }

        public int Column { get; init; }

        public bool IsApplicantTurn { get; init; }
    }
}
