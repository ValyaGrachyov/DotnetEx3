import { useParams } from "react-router";
import API from "../../httpclient";

function ExitGameButton({ iAmPlayer, onExit }) {
  const {roomId} = useParams();

  async function exit() {
    if (iAmPlayer)
      API.exitRoom(roomId);
    onExit();
  }

  return (
    <button onClick={() => exit()} className="exit-button">
      Exit Game
    </button>
  );
}

export default ExitGameButton;