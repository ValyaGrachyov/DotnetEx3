import { useState, useEffect, Children } from "react";
import Board from "./Board";
import ExitGameButton from "./ExitGameButton"
import GameStatusTitle from "./GameStatusTitle"
import GameState from "./GameState";
import { HubConnectionBuilder, LogLevel } from "@microsoft/signalr";
import { AuthData } from "../Auth/AuthWrapper";

const PLAYER_X = "X";
const PLAYER_O = "O";

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

function checkWinner(tiles, setStrikeClass, setGameState) {
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
        setGameState(GameState.playerXWins);
      } else {
        setGameState(GameState.playerOWins);
      }
      return;
    }
  }

  const areAllTilesFilledIn = tiles.every((tile) => tile !== null);
  if (areAllTilesFilledIn) {
    setGameState(GameState.draw);
  }
}

function TicTacToe() {
  const [strikeClass, setStrikeClass] = useState();

  const handleTileClick = (index) => {
    if (gameState !== GameState.myTurn) {
      return;
    }

    if (tiles[index] !== null) {
      return;
    }

    const newTiles = [...tiles];
    newTiles[index] = playerTurn;
    setTiles(newTiles);
    if (playerTurn === PLAYER_X) {
      setPlayerTurn(PLAYER_O);
    } else {
      setPlayerTurn(PLAYER_X);
    }
  };

  useEffect(() => {
    checkWinner(tiles, setStrikeClass, setGameState);
  }, [tiles]);

  return (
    <>
      <GameHubConnection/>
      <ExitGameButton/>
    </>
  );
}


function GameHubConnection() {
  const [tiles, setTiles] = useState(Array(9).fill(null));
  const [playerTurn, setPlayerTurn] = useState(PLAYER_X);
  const [gameState, setGameState] = useState(GameState.loading);
  

  const [connection, setConnection] = useState();
  const { token } = AuthData();

  function proccessGameEvent(event) {

  }

  async function createHubConnection() {
    const con = new HubConnectionBuilder()
      .withUrl(process.env.GAMEHUB, {
        accessTokenFactory: () => token
      })
      .configureLogging(LogLevel.Information)
      .withAutomaticReconnect()
      .build();

    con.on('GameEvent',
      (event) => {
         console.log(event);
     });
   
    con.on('RoomMessage', 
     (messageModel) => {console.log(messageModel)});
    await con.start();
    setConnection(con);
  }
  async function loadGameInfo() {

  }

  useEffect(() => {
    async function init() {
        await loadGameInfo();
        await createHubConnection();
    }
    init();

    return () => connection?.stop()
  }, []);


return(
  <>
      <Board
        putTile
        playerTurn={playerTurn}
        tiles={tiles}
        onTileClick={handleTileClick}
        strikeClass={strikeClass}
      />
      <GameStatusTitle/>
  </>);
}


export default TicTacToe;