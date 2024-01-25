import GameState from "./GameState";

function GameStatusTitle({ gameState }) {
  let message = null;

  switch (gameState) {
    case GameState.iWin:
        message = "You won game!";
        break;
    case GameState.opponentWins:
        message = "Opponent won game!";
        break;
    case GameState.myTurn:
        message = "Your turn.";
        break;
    case GameState.opponentTurn:
        message = "Opponent`s turn.";
        break;
    case GameState.inProgress:
        message = "You are watching the game.";
        break;
    default:
        break;

  }
  return message == null ? <></> : <div className="game-over">{message}</div>; 
}

export default GameStatusTitle;