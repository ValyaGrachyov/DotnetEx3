import { useState, useEffect, } from "react";
import Board from "./Board";
import ExitGameButton from "./ExitGameButton"
import { HubConnectionBuilder, LogLevel } from "@microsoft/signalr";
import { AuthData, getTokenFromSessionStorage } from "../Auth/AuthWrapper";
import ChatForm from "./ChatForm";
import { useNavigate, useParams } from "react-router-dom";
import API from "../../httpclient";
import "../../css/gameplay.css"

const PLAYER_X = "X";
const PLAYER_O = "O";
const EMPTY_BOARD = Array(9).fill(null);

function mapSymbol(symbolId) {
  if (symbolId === 1)
  return PLAYER_X;
  if (symbolId === 2)
  return PLAYER_O;
  return null;
}

const winningCombinations = [
  //Rows
  { combo: [0, 1, 2], strikeClass: "strike-row-1" },
  { combo: [3, 4, 5], strikeClass: "strike-row-2" },
  { combo: [6, 7, 8], strikeClass: "strike-row-3" },

  //Columns
  { combo: [0, 3, 6], strikeClass: "strike-column-1" },
  { combo: [1, 4, 7], strikeClass: "strike-column-2" },
  { combo: [2, 5, 8], strikeClass: "strike-column-3" },

  //Diagonals
  { combo: [0, 4, 8], strikeClass: "strike-diagonal-1" },
  { combo: [2, 4, 6], strikeClass: "strike-diagonal-2" },
];

function Game ({iAmPlayer}) {
  const navigate = useNavigate();
  const { roomId } = useParams();
  const { user } = AuthData();

  const [connection, setConnection] = useState(null);

  const [playerTurn, setPlayerTurn] = useState(null);
  const [strikeClass, setStrikeClass] = useState();
  const [tiles, setTiles] = useState(EMPTY_BOARD);
  const [mySymbol, setMySymbol] = useState(null);
  const [winner, setWinner] = useState(null);

  const [messages, setMessages] = useState([]);

  const makeTurn = (row, column) => {
    if (!iAmPlayer) {
        return;
    }

    if (!connection){
      alert("No connection, repeat action.");
    }

    connection?.invoke('MakeTurn', roomId, row, column);
  }

  const sendRoomMessage = (message) => {
    if (!connection){
      alert("No connection, repeat action.");
    }

    connection?.invoke("SendRoomMessage", roomId, message);
  }

  useEffect(() => {
    async function loadConnection() {
      try {
        const roomInfo = await API.getroom(roomId);
        if (roomInfo.currentGame) {
          const game = roomInfo.currentGame;

          setTiles(game.gameField.map(x => mapSymbol(x)));
          const turnSymbol =  game.isPlayer1Turn ? game.player1.symbol : game.player2.symbol;
          setPlayerTurn(mapSymbol(turnSymbol));

          if (iAmPlayer){
            if (game.player1.username === user.username)
            {
              setMySymbol(mapSymbol(game.player1.symbol));
            }
            else if (game.player2.username === user.username)
            {
              setMySymbol(mapSymbol(game.player2.symbol));
            }
          }
        }

        const newConnection = new HubConnectionBuilder()
                    .withUrl('https://localhost:7240/game-hub',{
                      accessTokenFactory: getTokenFromSessionStorage
                    })
                    .configureLogging(LogLevel.Information)
                    .withAutomaticReconnect()
                    .build();
        await newConnection.start();
        await newConnection.invoke('SubscribeRoomEvents', roomId);

        setConnection(newConnection);

        const myUsername = user.username;
        if (iAmPlayer && roomInfo.creatorUsername !== myUsername 
          && roomInfo.opponentUsername !== myUsername && !roomInfo.isBusy)
          await API.joinRoom(roomId);
      }
      catch(err) {
        navigate("/games");
      }
    }
    loadConnection();
  }, []);

  useEffect(() => {
      if (!connection)
        return;        
      connection.off('GameEvent');
      connection.off("RoomMessage");
      connection.on('GameEvent', event => proccessGameEvent(event));
      connection.on('RoomMessage', (username, message) => setMessages(prev => [ ...prev, {username:username, message:message}]));

  }, [connection, playerTurn, tiles, strikeClass, mySymbol, winner]);


  function proccessGameEvent(event) {
    switch (event.eventName)
    {
      case "NewGameStartEvent":
        proccessGameStart(event);
        break;
      case "PutSymbolGameEvent":
        proccessPutSymbol(event);
        break;
      case "GameEndEvent":
        proccessGameEnd(event);
        break;
      case "WrongArgumentEvent":
        if (iAmPlayer)
          proccessWrongArg(event);
        break;
      case "RoomWasClosedGameEvent":
        proccessGameClose(event);
        break;
    }

    function proccessWrongArg(event) {
      alert("Wrong arg");
    }

    function proccessGameStart(event) {
      setTiles(EMPTY_BOARD);
      setStrikeClass();
      setWinner(null);

      const player1E = event.player1;
      const player2E = event.player2;
      const player1Symbol = mapSymbol(player1E.symbol);
      const player2Symbol = mapSymbol(player2E.symbol);

      if (player1E.username === user.username) {
        setMySymbol(player1Symbol);
      }
      else if (player2E.username === user.username) {
        setMySymbol(player2Symbol);
      }
      else {
        setMySymbol(null);
      }
      setPlayerTurn(event.isPlayer1Turn ? player1Symbol : player2Symbol);
    }

    function proccessPutSymbol(event) {
      const symbol = mapSymbol(event.putSymbol);
      const tilesCopy = [...tiles];
      tilesCopy[event.row*3+event.column] = symbol;
      setTiles(tilesCopy);
      if (iAmPlayer){
        setPlayerTurn(playerTurn === PLAYER_X ? PLAYER_O : PLAYER_X);
      }
    }

    function proccessGameEnd(event) {
      if (event.winnerName){
        setWinner(event.winnerName);
        for (const { combo, strikeClass } of winningCombinations) {
          const tileValue1 = tiles[combo[0]];
          const tileValue2 = tiles[combo[1]];
          const tileValue3 = tiles[combo[2]];
      
          if (
            tileValue1 !== null &&
            tileValue1 === tileValue2 &&
            tileValue1 === tileValue3
          ) {
            setStrikeClass(strikeClass);
            if (tileValue1 === PLAYER_X) {
              console.log(PLAYER_X);
              //setGameState(GameState.XWin);
            } else {
              console.log(PLAYER_O);
              //setGameState(GameState.OWin);
            }
            return;
          }
        }
      }
      else {
        console.log("nobody");
      }
    }

    function proccessGameClose(event) {
      if (event.roomId == roomId)
        navigate("/games");
    }
  }
  
  return <div className="game-holder">
    <Board
      onTileClick={makeTurn}
      disabled={!iAmPlayer}
      tiles={tiles}
      thisPlayerSymbol={mySymbol}
      playerTurn={playerTurn}
      strikeClass={strikeClass}
    />
    <ChatForm messages={messages} sendMessage={sendRoomMessage}/>
  </div>
}

function TicTacToe({iAmPlayer = true}) {
  return (
    <section>
      <Game iAmPlayer={iAmPlayer}/>
      <ExitGameButton iAmPlayer={iAmPlayer}/>
    </section>
  );
}

  

export default TicTacToe;