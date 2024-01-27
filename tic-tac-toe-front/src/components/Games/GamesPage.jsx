import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import API from "../../httpclient";
import TicTacToe from "../Gameplay/TicTacToe";

function RoomList({selectRoom}) {
    const navigate = useNavigate();
    const [rooms, SetRooms] = useState([]);

    async function loadRooms() {
        
            const res = await API.getrooms();
            SetRooms(res.data);
    }

    useEffect(() => {
        loadRooms();
    },[])    

    
    return <>
            <div>
                <button onClick={() => {navigate("/leader-board")}} >Общий рейтинг</button>    
                        
                <div style={{border:20}}>
                    {rooms.map((x) =>    
                        <div >
                            <p>Creator {x.creatorUsername}</p>
                            <p>Creation Date {x.createdAtUtc}</p>
                            <p>Game Id {x.id}</p>
                            <button onClick={() => selectRoom(x.id, true)}>Join</button>
                            <button onClick={() => selectRoom(x.id, false)}>Watch</button>
                        </div>                                            
                    )}
                </div>
            </div>
        </>;
}


function GamesPage() {
    const [selectedRoom, setSelectedRoom] = useState();
    const [iAmPlayer, setIAmPlayer] = useState(false);

    const selectGame = (gameId, wannaPlay) => {
        setSelectedRoom(gameId);
        setIAmPlayer(wannaPlay);
    };

    return <>
    {selectedRoom && <TicTacToe roomId={selectedRoom} iAmPlayer={iAmPlayer} />}
    {!selectedRoom && <RoomList selectRoom={selectGame}/>}
    </>
}

export default GamesPage;