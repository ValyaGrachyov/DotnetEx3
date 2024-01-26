import "../../css/login-page.css"
import { AuthData } from "./AuthWrapper";
import { useNavigate } from "react-router-dom";
import React, { useReducer, useState, useEffect } from "react";
import { isNullOrEmptyString } from "../../commonFunctions";
import API from "../../httpclient";

function validateFields({username, password, passwordRepetition}) {
    if (isNullOrEmptyString(username))
        return "Username is required.";

    if (isNullOrEmptyString(password))
        return "Password is required.";

    if (isNullOrEmptyString(passwordRepetition))
        return "Repeat the password.";
    
    if (passwordRepetition !== password)
        return "Passwords does not match.";
    
    return null;
}   

export default function RegistrationPage () {
    const { user } = AuthData();
    const navigate = useNavigate();
    const [ formData, setFormData ] = useReducer((formData, newItem) => { return ( {...formData, ...newItem} )}, 
        {username: null, password: null, passwordRepetition: null});
    const [ errorMessage, setErrorMessage ] = useState(null);

    const executeRegistration = async () => {
        try {
            const formValidationMessage = validateFields(formData);
            if (formValidationMessage != null){
                setErrorMessage(formValidationMessage);
                return;
            }

            await API.register(formData.username, formData.password);
            navigate("/login");
        } catch (error) {
            setErrorMessage(error.message);
        }
   }

   useEffect(() => {
        if (user?.isAuthenticated)
            navigate("/games");
    });

    return (
    <div className="form-holder">
        <h2>Create New Account</h2>
        <div className="inputs">
            <div className="input">
                <input type="text" maxLength="256" minLength="6" placeholder="username"
                       required={true}
                       value={formData?.username} 
                       onChange={(e) => setFormData({username: e.target.value?.trim()}) }/>
            </div>
            <div className="input">
                 <input placeholder="password" type="password"
                       required={true}
                       value={formData?.password}
                       onChange={(e) => setFormData({password: e.target.value?.trim()}) }/>
            </div>
            <div className="input">
                 <input placeholder="repeat password" type="password"
                       required={true}
                       value={formData?.passwordRepetition}
                       onChange={(e) => setFormData({passwordRepetition: e.target.value?.trim()}) }/>
            </div>
            <div className="button-section">
                 <button onClick={executeRegistration}>Register</button>
            </div>
            {errorMessage ? <div className="error">{errorMessage}</div> : null }
        </div>
    </div>);
}