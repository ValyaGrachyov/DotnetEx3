import React from "react"
import LoginPage from "./Auth/LoginPage";
import Logout from "./Auth/Logout";
import RegistrationPage from "./Auth/RegistrationPage";
import TicTacToe from "./Gameplay/TicTacToe"
import LeaderBoardPage from "./LeaderBoard/LeaderBoardPage";
import NotFoundPage from "./NotFoundPage";
import RoomsListingPage from "./RoomsListing/RoomsListingPage";

export const routesList = [
    { path: "/games/:roomId", name: "The Game", element: <TicTacToe/>, isNavigationBar: false, isPrivate: true },
    //private nav bar items
    { path: "/games", name: "Games List", element: <RoomsListingPage/>, isNavigationBar: true, isPrivate: true },
    { path: "/leader-board", name: "Leader Board", element: <LeaderBoardPage/>, isNavigationBar: true,  isPrivate: true },
    { path: "/log-out", name: "Log out", element: <Logout/>, isNavigationBar: true, isPrivate: true },
    // public routes
    { path: "/login", name: "Login", element: <LoginPage/>, isNavigationBar:false, isPrivate: false },
    { path: "/register", name: "Registration", element: <RegistrationPage/>, isNavigationBar:false, isPrivate: false },
    //other
    { path: "*", name: "Not Found", element: <NotFoundPage/>, isNavigationBar: false, isPrivate: false },
]

export default routesList;