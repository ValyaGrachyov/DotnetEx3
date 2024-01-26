import { AuthData } from "../Auth/AuthWrapper";
import GameState from "./GameState";

function GameStatusTitle({ gameState, winnerNickName }) {
  const {user} = AuthData();
  let message = null;
    if (gameState == GameState.noBodyWins)
    {
        message = "Nobody wins!";
    }
    
    if (winnerNickName) {
        message = `${winnerNickName} won game!`
    }
    
    if (GameState.myTurn)
    {
        message = "Your turn.";
    }
    if (GameState.opponentTurn)
    {
        message = "Opponent`s turn.";
    }
    if (GameState.inProgress)
    {
        message = "You are watching the game."
    }

  return message ? <></> : <div className="game-over">{message}</div>; 
}

export default GameStatusTitle;