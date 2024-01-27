import { useNavigate, useParams } from "react-router";
import API from "../../httpclient";

function ExitGameButton({ iAmPlayer }) {
  const {roomId} = useParams();
  const navigate = useNavigate();

  async function exit() {
    if (iAmPlayer)
      API.exitRoom(roomId);
    navigate("/games");
  }

  return (
    <button onClick={() => exit()} className="exit-button">
      Exit Game
    </button>
  );
}

export default ExitGameButton;