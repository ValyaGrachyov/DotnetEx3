import { Route, Routes, Link } from "react-router-dom";
import routesList from "./navigation"; 
import { AuthData } from "./Auth/AuthWrapper";
import { useLocation } from "react-router-dom";

export const RenderRoutes = () => {
    const { user } = AuthData();

    return <Routes>
            {
                routesList.map((r,i) => {
                    if (r.isPrivate && user.isAuthenticated)
                        return <Route key={i} path={r.path} element={r.element}/>;
                    else if (!r.isPrivate)
                        return <Route key={i} path={r.path} element={r.element}/>;
                    return false;
                })
            }
        </Routes>;
}

export const RenderNavBar = () => {
    const location = useLocation();
    const { user, logout } = AuthData();

    const MenuItem = ({r, toggled}) => {
         return (
              <div className={toggled ? "menuItem-toggled" : "menuItem"}><Link to={r.path}>{r.name}</Link></div>
         )
    }

    return (
         <div className="menu">
              { routesList.map((r, i) => {
                   const isToggled = location === r.path;
                   if (!r.isPrivate && r.isMenu)
                        return <MenuItem key={i} r={r} isToggled={isToggled}/>;
                   else if (user.isAuthenticated && r.isMenu)
                        return <MenuItem key={i} r={r} isToggled={isToggled}/>;
                   return false;
              })}

              { user.isAuthenticated ?
              <div className="menuItem"><Link to={'#'} onClick={logout}>Log out</Link></div>
              : <></>}
         </div>
    )
}