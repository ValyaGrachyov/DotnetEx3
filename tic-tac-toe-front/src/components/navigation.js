import React from "react"
import LoginPage from "./Auth/LoginPage";
import Logout from "./Auth/Logout";
import RegistrationPage from "./Auth/RegistrationPage";
import NotFoundPage from "./NotFoundPage";
import GamesPage from "./Games/GamesPage";

export const routesList = [
    //private nav bar items
    { path: "/games", name: "Games List", element: <GamesPage/>, isNavigationBar: true, isPrivate: true },
    { path: "/log-out", name: "Log out", element: <Logout/>, isNavigationBar: true, isPrivate: true },
    // public routes
    { path: "/login", name: "Login", element: <LoginPage/>, isNavigationBar:false, isPrivate: false},
    { path: "/registration", name: "Registration", element: <RegistrationPage/>, isNavigationBar:false, isPrivate: false},
    //other
    { path: "*", name: "Not Found", element: <NotFoundPage/>, isNavigationBar: false, isPrivate: false },
]

export default routesList;