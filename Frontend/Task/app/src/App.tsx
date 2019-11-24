import React from "react";
import logo from "./logo.svg";
import "./App.css";
import { CurrencyRates } from "./components/CurrencyRates";
import CurrencyRatesContainer from "./components/CurrencyRatesContainer";

const App: React.FC = () => {
  return <CurrencyRatesContainer />;
};

export default App;
