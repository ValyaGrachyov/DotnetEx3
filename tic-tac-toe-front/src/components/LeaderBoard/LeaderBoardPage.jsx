import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import API from "../../httpclient";

function LeaderBoardPage() {

    const navigate = useNavigate();
    const [rates, SetRates] = useState([]);

    async function loadRates() {
        
            var res = await API.getUsersRaiting();
            SetRates(res.data.value);
            console.log(rates);
    }

    useEffect(() => {
        loadRates();
    },[])    

    return (
        <div>
            <button onClick={() => {navigate("/games")}}>К выбору комнат</button>
            <p>Таблица лидеров:</p>
            {rates.map((el) => 
            <div>
                <p>{el.username} : {el.rate}</p>                
            </div>
            
            )}

        </div>
    );
}

export default LeaderBoardPage;