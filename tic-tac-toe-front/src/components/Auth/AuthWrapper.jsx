import { createContext, useContext, useState } from "react"
import { RenderRoutes, RenderNavBar } from "../RenderRoutes";
import Header from "../Header";

const AuthContext = createContext();
export const AuthData = () => useContext(AuthContext);


export const AuthWrapper = () => {

     const [ user, setUser ] = useState({name: "", isAuthenticated: true})

     const login = (userName, password) => {

          // Make a call to the authentication API to check the username
          
          return new Promise((resolve, reject) => {

               if (password === "password") {
                    setUser({name: userName, isAuthenticated: true})
                    resolve("success")
               } else {
                    reject("Incorrect password")
               }
          })
          
          
     }
     const logout = () => {

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