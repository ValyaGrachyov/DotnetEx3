import Strike from "./Strike";
import Tile from "./Tile";

function Board({ tiles, onTileClick, thisPlayerSymbol, playerTurn, strikeClass, disabled }) {

  function handleTileClick(row, column) {
    if (disabled || thisPlayerSymbol !== playerTurn)
      return;
    if (row < 0 || row > 2 || column < 0 || column > 2)
      return;
    const index = row * 3 + column;
    if (tiles[index] !== null)
      return;

    onTileClick(row, column);
  }

  return (
    <div className="board">
      <Tile
        playerTurn={playerTurn}
        onClick={() => handleTileClick(0,0) }
        value={tiles[0]}
        className="right-border bottom-border"
      />
      <Tile
        playerTurn={playerTurn}
        onClick={() => handleTileClick(0,1)}
        value={tiles[1]}
        className="right-border bottom-border"
      />
      <Tile
        playerTurn={playerTurn}
        onClick={() => handleTileClick(0,2)}
        value={tiles[2]}
        className=" bottom-border"
      />
      <Tile
        playerTurn={playerTurn}
        onClick={() => handleTileClick(1,0)}
        value={tiles[3]}
        className="right-border bottom-border"
      />
      <Tile
        playerTurn={playerTurn}
        onClick={() => handleTileClick(1,1)}
        value={tiles[4]}
        className="right-border bottom-border"
      />
      <Tile
        playerTurn={playerTurn}
        onClick={() => handleTileClick(1,2)}
        value={tiles[5]}
        className="bottom-border"
      />
      <Tile
        playerTurn={playerTurn}
        onClick={() => handleTileClick(2,0)}
        value={tiles[6]}
        className="right-border"
      />
      <Tile
        playerTurn={playerTurn}
        onClick={() => handleTileClick(2,1)}
        value={tiles[7]}
        className="right-border"
      />
      <Tile
        playerTurn={playerTurn}
        onClick={() => handleTileClick(2,2)}
        value={tiles[8]}
      />
      <Strike strikeClass={strikeClass} />
    </div>
  );
}

export default Board;