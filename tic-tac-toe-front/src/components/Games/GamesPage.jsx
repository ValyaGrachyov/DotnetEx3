import { useState } from "react";
import { useNavigate } from "react-router-dom";
import InfinityScroll from "./InfinityScroll";

function GamesPage() {

    const navigate = useNavigate();

    const onClick = (roomId) => {

        navigate(`/games/${roomId}`);
    }


    return <>
            <div>
                <div>room 1
                    <button onClick={navigate("dsfsdf")}>Join</button>
                </div>
                <div>room 2</div>
            </div>
        </>;
}

export default GamesPage;