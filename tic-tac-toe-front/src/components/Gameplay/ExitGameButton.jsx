import API from "../../httpclient";

function ExitGameButton({ onExit }) {
  async function exit() {
    onExit();
  }

  return (
    <button onClick={() => exit()} className="exit-button">
      Exit Game
    </button>
  );
}

export default ExitGameButton;