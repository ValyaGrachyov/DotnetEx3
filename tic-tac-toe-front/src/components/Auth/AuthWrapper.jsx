import { createContext, useContext, useEffect, useState } from "react"
import { RenderRoutes, RenderNavBar } from "../RenderRoutes";
import Header from "../Header";
import API from "../../httpclient";
import { useLocation, useNavigate } from "react-router-dom";

const getTokenFromSessionStorage = () => sessionStorage.getItem("token");

const AuthContext = createContext();
export const AuthData = () => useContext(AuthContext);

export const AuthWrapper = () => {
     const location = useLocation();
     const navigate = useNavigate();
     const [ user, setUser ] = useState({username: sessionStorage.getItem("username") ?? "", isAuthenticated: getTokenFromSessionStorage() != undefined })

     const login = (username, password) => {
        return API.login(username, password)
        .then(() => setUser({...user, username: sessionStorage.getItem("username"), isAuthenticated: true}));
     }

     const getToken = () => getTokenFromSessionStorage();

     const logout = () => {
          API.logout();
          setUser({...user, isAuthenticated: false})
     }

     useEffect(() => {
          async function validateToken() {
               const isValidSession = await API.testSession();
               if (isValidSession == false){
                    logout();
                    if (location != "/registration")
                         navigate("/login");
               }
          }

          validateToken();
     }, []);

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