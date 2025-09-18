import { Outlet } from "react-router-dom";
import { GlobalStyles } from "./styles/GlobalStyles";

export default function App() {
  return (
    <>
      <GlobalStyles />
      <Outlet />
    </>
  );
}
