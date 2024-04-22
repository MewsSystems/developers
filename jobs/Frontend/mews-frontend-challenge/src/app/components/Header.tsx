import { Link } from "react-router-dom";

import { getSearchRoute } from "../AppRouter.utils";
import logo from "./assets/logo.png";

export function Header() {
  return (
    <header className="flex items-center justify-between bg-slate-900 px-4 py-6 text-white md:px-8">
      <Link to={getSearchRoute()}>
        <img className="w-24" src={logo} alt="Mews logo" />
      </Link>
      <h1 className="font-bold">Mews frontend challenge</h1>
    </header>
  );
}
