import React from "react";
import { PropsFromRedux } from "./CurrencyRatesContainer";
import Loading from "./Loading";
import ActiveRate from "./ActiveRate";

export const CurrencyRates: React.FC<PropsFromRedux> = ({
  currencyPairs,
  configLoaded,
  firstRatesLoaded,
  toggleVisibility
}) => {
  let message = null;
  if (!configLoaded) {
    message = "Loading configuration";
    return <Loading message={message} />;
  } else if (!firstRatesLoaded) {
    message = "Loading rates";
    return <Loading message={message} />;
  } else {
    const active = Object.keys(currencyPairs)
      .filter(id => currencyPairs[id].shown)
      .map(id => (
        <ActiveRate
          id={id}
          currencyPair={currencyPairs[id]}
          toggleVisibility={toggleVisibility}
        />
      ));
    const passive = Object.keys(currencyPairs)
      .filter(id => !currencyPairs[id].shown)
      .map(id => (
        <ActiveRate
          id={id}
          currencyPair={currencyPairs[id]}
          toggleVisibility={toggleVisibility}
        />
      ));
    return (
      <div>
        Aktívne
        {active}
        Pasívne {passive}
      </div>
    );
  }
};
