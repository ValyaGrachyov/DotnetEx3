import { createContext, useContext, useState } from "react"
import { RenderRoutes, RenderNavBar } from "../RenderRoutes";
import Header from "../Header";
import API from "../../httpclient";

const AuthContext = createContext();
export const AuthData = () => useContext(AuthContext);

export const AuthWrapper = () => {

     const [ user, setUser ] = useState({username: "", isAuthenticated: true})

     const login = (username, password) => {
        return API.login(username, password)
        .then(r => setUser({...user, username: r.username, isAuthenticated: true}));
     }

     const logout = () => {
          API.logout();
          setUser({...user, isAuthenticated: false})
     }

     return (
               <AuthContext.Provider value={{user, login, logout}}>
                    <>
                         <Header />
                         <RenderNavBar/>
                         <RenderRoutes />
                    </>
               </AuthContext.Provider>
     )
}