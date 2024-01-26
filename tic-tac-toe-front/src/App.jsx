import "./App.css";
import { BrowserRouter } from "react-router-dom";
import { AuthWrapper } from "./components/Auth/AuthWrapper";

function App() {

  return <BrowserRouter>
          <AuthWrapper />
        </BrowserRouter>
}

export default App;