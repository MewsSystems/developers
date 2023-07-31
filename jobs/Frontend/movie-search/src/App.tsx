import { FC, useEffect } from "react";
import { useRoutes } from "react-router-dom";
import "./App.css";
import { routes } from "./routes/routes";
import { getConfigThunk } from "./store/config-thunks";
import { useAppDispatch } from "./store/store";

const App: FC<{}> = () => {
  const dispatch = useAppDispatch();
  let configuredRoutes = useRoutes(routes);
  useEffect(()=>{

    dispatch(getConfigThunk())

  },[dispatch])

  return <div className='App'>{configuredRoutes}</div>;
};

export default App;
