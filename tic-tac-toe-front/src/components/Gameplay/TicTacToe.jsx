import { useState, useEffect, Children } from "react";
import Board from "./Board";
import ExitGameButton from "./ExitGameButton"
import GameStatusTitle from "./GameStatusTitle"
import GameState from "./GameState";
import { HubConnectionBuilder, LogLevel } from "@microsoft/signalr";
import { AuthData, getTokenFromSessionStorage } from "../Auth/AuthWrapper";
import { useParams } from "react-router-dom";

const PLAYER_X = "X";
const PLAYER_O = "O";
const DEFAULT_PLAYER = {username:"unknown", symbol:"NaN"};
const EMPTY_BOARD = Array(9).fill(null);

function mapSymbol(symbolId) {
  if (symbolId === 1)
  return PLAYER_X;
  if (symbolId === 2)
  return PLAYER_O;
  return "NaN";
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

function TicTacToe() {
  return (
    <>
      <GameHubConnection iAmPlayer={true}/>
      <ExitGameButton/>
    </>
  );
}


function GameHubConnection({iAmPlayer}) {
  const [awaitingAction, setAwaitingAction] = useState();
  const [mySymbol, setMySymbol] = useState(null);
  const [tiles, setTiles] = useState(EMPTY_BOARD);
  const [playerTurn, setPlayerTurn] = useState(PLAYER_X);
  const [player1, setPlayer1] = useState(DEFAULT_PLAYER);
  const [player2, setPlayer2] = useState(DEFAULT_PLAYER);
  const [strikeClass, setStrikeClass] = useState(null);
  const [winnerUsername, setWinnerUsername] = useState(null);

  const [gameState, setGameState] = useState(GameState.waitingForOpponent);
  const { roomId } = useParams();

  const [connection, setConnection] = useState();
  const { user } = AuthData();

  function resetGame() {
    setPlayer1(DEFAULT_PLAYER);
    setPlayer2(DEFAULT_PLAYER);
    setPlayerTurn(PLAYER_X);
    setTiles(EMPTY_BOARD);
    setMySymbol(null);
    setGameState(GameState.loading);
    setWinnerUsername(null);
    setStrikeClass(null);
    setAwaitingAction(null);
  }


  function proccessGameStart(event) {
    resetGame();

    const player1E = event.player1;
    const player2E = event.player2;
    const player1Symbol = mapSymbol(player1E.symbol);
    const player2Symbol = mapSymbol(player2E.symbol);
    setPlayer1({...player1, username: player1E.userName, symbol:player1Symbol});
    setPlayer2({...player2, username: player2E.userName, symbol: player2Symbol});
    setPlayerTurn(event.isPlayer1Turn ? player1Symbol : player2Symbol);
    setTiles(EMPTY_BOARD);
    if (iAmPlayer) {
      if (player1E.userName == user.username)
          setMySymbol(player1Symbol);
      else if (player2E.userName == user.username)
          setMySymbol(player2Symbol);
      else
          setMySymbol(null);
    }
  }

  function proccessTurnSwitch(event) {
    if (event.waitingForUser == user.username)
    {
      setPlayerTurn(mySymbol);
      setGameState(GameState.myTurn);
    }
    else
      setGameState(GameState.opponentTurn);
  }

  function proccessPutSymbol(event) {
    const symbol = mapSymbol(event.putSymbol);
    const tilesCopy = [...tiles];
    tilesCopy[event.row*3+event.column] = symbol;
    setTiles(tilesCopy);

    if (awaitingAction){
      setAwaitingAction();
    }
  }

  function proccessGameEnd(event) {
    if (event.winnerName){
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
            setGameState(GameState.XWin);
          } else {
            setGameState(GameState.OWin);
          }
          return;
        }
      }
    }
    else {
      setGameState(GameState.noBodyWins);
    }
  }

  function proccessWrongArg(event) {
    alert();
  }

  async function handleTileClick(row, column) {
    if (!iAmPlayer || playerTurn !== mySymbol)
      return;
    if (row < 0 || row > 2 || column < 0 || column > 2)
      return;
    const index = row * 3 + column;
    if (tiles[index] !== null)
      return;

    if (connection == null)
    {
      alert("Fatal error. No Connection");
      return;
    } 

    setAwaitingAction({roomId: roomId, row: row, column: column, symbol: mySymbol})
    await connection?.invoke("makeTurn", roomId, row, column);

    const tilesCopy = [...tiles];
    const indx = row * 3 + column;
    tilesCopy[indx] = mySymbol;
    setTiles(tilesCopy);
  }

  function proccessGameEvent(event) {
    switch (event.eventName)
    {
      case "NewGameStartEvent":
        proccessGameStart(event);
        break;
      case "TurnSwitchEvent":
        proccessTurnSwitch(event)
      case "PutSymbolGameEvent":
        proccessPutSymbol(event);
        break;

      case "GameEndEvent":
        proccessGameEnd(event);
        break;
      case "WrongArgumentEvent":
        proccessWrongArg(event);
        break;
    }
  }


  async function createHubConnection() {
    const con = new HubConnectionBuilder()
      .withUrl("https://localhost:7240/game-hub" /*process.env.GAMEHUB*/, {
        accessTokenFactory: getTokenFromSessionStorage
      })
      .configureLogging(LogLevel.Information)
      .withAutomaticReconnect()
      .build();

    con.on('GameEvent', event => proccessGameEvent(event));

    con.on('RoomMessage', 
     (username, message) => {console.log(`${username}: ${message}`)});
    await con.start();
    setConnection(con);

    con.invoke("subscribeRoomEvents", roomId);
  }

  async function loadGameInfo() {

  }

  useEffect(() => {
    async function init() {
        await loadGameInfo();
        await createHubConnection();
    }
    init();

    return () => {
      connection?.invoke("unsubscribeRoomEvents", roomId);
      connection?.stop();
    }
  }, []);
const disabled = !iAmPlayer ||  playerTurn !== mySymbol;
return(
  <>
      <Board
        playerTurn={playerTurn}
        tiles={tiles}
        disabled={false}
        onTileClick={handleTileClick}
        strikeClass={strikeClass}
      />
      <GameStatusTitle gameState={gameState}  winnerNickName={winnerUsername}/>
  </>);
}


export default TicTacToe;