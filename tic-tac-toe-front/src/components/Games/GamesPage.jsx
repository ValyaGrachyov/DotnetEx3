import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import API from "../../httpclient";

function GamesPage() {

    const navigate = useNavigate();
    const [rooms, SetRooms] = useState([]);

    async function loadRooms() {
        
            var res = await API.getrooms();
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
                            <p>Создатель {x.creatorUsername}</p>
                            <p>Время создания {x.createdAtUtc}</p>
                            <p>id комнаты {x.id}</p>
                            <button onClick={() => {navigate(x.id)}}>Join</button>
                        </div>                                            
                    )}
                </div>
            </div>
        </>;
}

export default GamesPage;