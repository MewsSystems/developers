import { useRoutes } from "react-router-dom";
import "./App.css";
import { routes } from "./routes/routes";

function App() {

  
  let configuredRoutes = useRoutes(routes);

  return <div className='App'>{configuredRoutes}</div>;
}

export default App;
