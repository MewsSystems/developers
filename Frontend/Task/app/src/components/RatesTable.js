import React from "react";
import { Dots } from "react-activity";

const RatesTable = props => {
  const { data, changeFilter } = props;

  return (
    <div>
      {data.map((item, index) => (
        <div
          className="Rtable4--cols"
          onClick={() => {
            changeFilter(item.id);
          }}
          key={item.id}
        >
          <div className="Rate-cell0">
            <div>
              <b>{index + 1}</b>
            </div>
          </div>
          <div className="Rate-cell1">
            {item.config[0].code + ":" + item.config[0].name}
          </div>
          <div className="Rate-cell2">
            {item.config[1].code + ": " + item.config[1].name}
          </div>
          <div className="Rate-cell3">{item.rate}</div>
          <div className="Rate-cell4">{item.trend ? item.trend : <Dots />}</div>
        </div>
      ))}
    </div>
  );
};

export default RatesTable;
