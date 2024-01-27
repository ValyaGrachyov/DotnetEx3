import { useState } from "react";
import API from "../../httpclient";
import TicTacToe from "../Gameplay/TicTacToe";
import InfinityScroll from "./InfinityScroll";
import "../../css/rooms.css"
import { Navigate } from "react-router-dom";
import LeaderBoardPage from "../LeaderBoard/LeaderBoardPage";
import { AuthData } from "../Auth/AuthWrapper";

const RoomListElement = ({roomInfo, index, onJoinClick, onWatchClick}) => <>
    <div key={index} className="room-list-elem">
        <div className="room-list-elem-number">{index+1}</div>
        <div className="room-list-elem-creator">{roomInfo.creatorUsername}</div>
        <div>{roomInfo.id}</div>
        <div className="room-list-elem-creationDate">{new Date(roomInfo.createdAtUtc).toISOString().slice(0, 16).replace('T', ' ')}</div>
        <div className="room-list-elem-roomStage">{roomInfo.isBusy ? "in progress" : "waiting for opponent"}</div>
        <div>{`Max rate: ${roomInfo.maxAllowedUserRating}`}</div>
        <button className="room-list-elem-joinButton" onClick={() => {onJoinClick(roomInfo.id)}} >join</button>
        <button className="room-list-elem-joinButton watch" onClick={() => {onWatchClick(roomInfo.id)}}>watch</button>
    </div>
</>

function RoomList({selectRoom}) {
    const elementsPerPage = 10;
    
    function loadRoomInfos(page) {
        return API.getrooms(page, elementsPerPage)
            .then(response => { return {newData: response.requestedRooms, totalCount: response.totalRooms}; });
    }

    const onJoinClick = (roomId) => selectRoom(roomId, true);

    const onWatchClick = (roomId) => selectRoom(roomId, false);

    return (
            <div className="room-list">       
                <InfinityScroll
                    elementsPerPage={elementsPerPage}
                    demandAdditionData={loadRoomInfos}
                    mapDisplayData={(roomInfo, key) => 
                        RoomListElement({roomInfo:roomInfo, index: key, onJoinClick: onJoinClick, onWatchClick: onWatchClick})}
                 />
            </div>
        );
}


function GamesPage() {
    const { user } = AuthData();
    const [selectedRoom, setSelectedRoom] = useState(null);
    const [iAmPlayer, setIAmPlayer] = useState(false);
    const [leadersPageOpen, setLeadersPageOpen] = useState(false);
    const [gameCreationPageOpen, setGameCreationPageOpen] = useState(false);

    const selectGame = (gameId, wannaPlay) => {
        setSelectedRoom(gameId);
        setIAmPlayer(wannaPlay);
    };

    const onExitRoom = () => {
        setSelectedRoom(null);
        setIAmPlayer(false);
        setGameCreationPageOpen(false);
        setLeadersPageOpen(false);
    }

    const closeGameCreationPage = () => setGameCreationPageOpen(false);

    return <>
        {!user.isAuthenticated && <Navigate to="/login"/> }
        {user.isAuthenticated && 
          <>
             <div className="buttnos-holder">
                <div className="leader-board-holder" onClick={() => setLeadersPageOpen(prev => !prev)}>
                    <img width="32" height="32" src="https://img.icons8.com/pastel-glyph/64/40C057/statistic-file--v1.png" alt={leadersPageOpen ? "Close Leader Board" : "Open Leader Board"}/>
                    {!leadersPageOpen && !gameCreationPageOpen && !selectedRoom && 
                    <img onClick={() => setGameCreationPageOpen(true)} width="32" height="32" src="https://img.icons8.com/pulsar-line/32/40C057/filled-plus-2-math.png" alt="Create A Game"/>}
                </div>
             </div>
             {
                 !leadersPageOpen && 
                 <>
                    {  gameCreationPageOpen && <></>}
                    { !gameCreationPageOpen && 
                        <>
                            {selectedRoom && <TicTacToe roomId={selectedRoom} onExitRoom={onExitRoom} iAmPlayer={iAmPlayer} />}
                            {!selectedRoom && <RoomList selectRoom={selectGame}/>}
                        </>
                    }
                 </>
             }
             {leadersPageOpen && <LeaderBoardPage />}
         </>
        }
    </>
}

export default GamesPage;