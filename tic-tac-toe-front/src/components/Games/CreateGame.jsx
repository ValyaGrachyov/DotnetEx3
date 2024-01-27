import "../../css/login-page.css"
import { Link, useNavigate } from "react-router-dom";
import React, { useEffect, useReducer, useState } from "react";
import API from "../../httpclient";
import GamesPage from "./GamesPage";




export default function CreateGame ({closeGameCreationPage, setSelectedRoom}) {
    const [ formData, setFormData ] = useState({maxRate: "" });
    const [ errorMessage, setErrorMessage ] = useState(null);

    const executeCreateGame = async () => {
        try {
            const formValidationMessage = validateField(formData);
            if (formValidationMessage != null){
                setErrorMessage(formValidationMessage);
                return;
            }
            const roomId = await API.createroom(formData.maxRate);
            setSelectedRoom(roomId);
            closeGameCreationPage();

        } catch (error) {
             setErrorMessage(error?.message);
        }
   }    

   function validateField(maxRate) {
    if (typeof maxRate === 'number' || maxRate < 0)
        return "Incorrect rate";
    
    return null;
}   

    return (
    <div className="form-holder">
        <h2>Create room</h2>
        <div className="inputs">
            <div className="input">
                <input type="text" placeholder="number" required={true}
                       value={formData?.maxRate} 
                       onChange={(e) => setFormData({maxRate: e.target.value?.trim()}) }/>
            </div>            
            <div className="button-section">
                 <button onClick={executeCreateGame}>Create</button>                 
            </div>
            <button onClick={() => closeGameCreationPage()}>Close</button>
            {errorMessage ? <div className="error">{errorMessage}</div> : null }
        </div>
    </div>);
}