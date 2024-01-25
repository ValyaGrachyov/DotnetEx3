import GameState from "./GameState";

function ExitGameButton({ gameState, onReset }) {
  if (gameState === GameState.inProgress) {
    return;
  }
  return (
    <button onClick={onReset} className="exit-button">
      Exit Game
    </button>
  );
}

export default ExitGameButton;