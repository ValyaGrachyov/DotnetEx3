import { Navigate, useLocation } from "react-router-dom";
import { AuthData } from "./Auth/AuthWrapper";

function NotFoundPage() {
    const location = useLocation();
    const {user} = AuthData();
    if (location === "/games" && !user.isAuthenticated)
        return <Navigate to="/login"/>

    return <h1>Page Not Found</h1>
}
export default NotFoundPage;