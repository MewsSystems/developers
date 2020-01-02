import React from "react";
import RatesList from "./components/rates-list/rates-list.component";
import Header from "./components/header/header.component";
import "./App.css";
import { library } from "@fortawesome/fontawesome-svg-core";
import {
  faArrowRight,
  faArrowDown,
  faArrowUp
} from "@fortawesome/free-solid-svg-icons";

library.add(faArrowRight, faArrowUp, faArrowDown);

const App: React.FC = () => {
  return (
    <>
      <Header />
      <RatesList />
    </>
  );
};

export default App;
