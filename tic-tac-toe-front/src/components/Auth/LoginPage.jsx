import "../../css/login-page.css"
import { AuthData } from "./AuthWrapper";
import { Link, useNavigate } from "react-router-dom";
import React, { useEffect, useReducer, useState } from "react";
import { isNullOrEmptyString } from "../../commonFunctions";

function validateFields({username, password}) {
    if (isNullOrEmptyString(username))
        return "Username is required.";

    if (isNullOrEmptyString(password))
        return "Password is required.";
    
    return null;
}   

export default function LoginPage () {
    const navigate = useNavigate();
    const { user, login } = AuthData();
    const [ formData, setFormData ] = useReducer((formData, newItem) => { return ( {...formData, ...newItem} )}, {username: "", password: ""});
    const [ errorMessage, setErrorMessage ] = useState(null);

    const executeLogin = async () => {
        try {
            const formValidationMessage = validateFields(formData);
            if (formValidationMessage != null){
                setErrorMessage(formValidationMessage);
                return;
            }

            await login(formData.username, formData.password);
            navigate("/games");

        } catch (error) {
             setErrorMessage(error?.message);
        }
   }

    useEffect(() => {
        if (user?.isAuthenticated)
            navigate("/games");
    });

    return (
    <div className="form-holder">
        <h2>Sign Into Account</h2>
        <div className="inputs">
            <div className="input">
                <input type="text" maxLength="256" minLength="6" placeholder="username" required={true}
                       value={formData?.username} 
                       onChange={(e) => setFormData({username: e.target.value?.trim()}) }/>
            </div>
            <div className="input">
                 <input placeholder="password" type="password" required={true}
                       value={formData?.password} 
                       onChange={(e) => setFormData({password: e.target.value?.trim()}) }/>
            </div>
            <div className="button-section">
                 <button onClick={executeLogin}>Log in</button>
                 <Link to="/registration" className="register-link">Registration</Link>
            </div>
            {errorMessage ? <div className="error">{errorMessage}</div> : null }
        </div>
    </div>);
}