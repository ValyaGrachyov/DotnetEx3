using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.TicTacToe;

public enum TicTacToeGameState
{
    WaitingForOpponent,
    Closed,
    Started,
    RestartCooldown
}
