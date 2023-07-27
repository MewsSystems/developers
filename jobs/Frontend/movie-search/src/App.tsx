import { FC } from "react";
import { useRoutes } from "react-router-dom";
import "./App.css";
import { routes } from "./routes/routes";

const App: FC<{}> = () => {
  let configuredRoutes = useRoutes(routes);

  return <div className='App'>{configuredRoutes}</div>;
};

export default App;
