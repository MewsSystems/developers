import React from "react";
import { PropsFromRedux } from "./CurrencyRatesContainer";
import Loading from "./Loading";
import ActiveRate from "./ActiveRate";
import PassiveRate from "./PassiveRate";

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
          key={"active-" + id}
          id={id}
          currencyPair={currencyPairs[id]}
          toggleVisibility={toggleVisibility}
        />
      ));
    const passive = Object.keys(currencyPairs)
      .filter(id => !currencyPairs[id].shown)
      .map(id => (
        <PassiveRate
          key={"passive-" + id}
          id={id}
          currencyPair={currencyPairs[id]}
          toggleVisibility={toggleVisibility}
        />
      ));
    return (
      <div className="flexContainer">
        <div className="shrink2">
          <div>
            <div className="flexRow">
              {" "}
              <h1>Visible currency rates</h1>
            </div>
          </div>

          <div>
            <div className="flexRow">{active}</div>
          </div>
        </div>

        <div className="shrink1">
          <div className="flexRow">
            <h1>Hidden currency rates</h1>
          </div>
          <div className="flexRow">{passive}</div>
        </div>
      </div>
    );
  }
};
