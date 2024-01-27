import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import API from "../../httpclient";

function LeaderBoardPage() {

    const [rates, setRates] = useState([]);

    async function loadRates() {
        
            var res = await API.getUsersRaiting();
            setRates(res.data.value);
            console.log(rates);
    }

    useEffect(() => {
        loadRates();
    },[])    

    return (
        <div>
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