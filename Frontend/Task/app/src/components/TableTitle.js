import React from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faFilter } from "@fortawesome/free-solid-svg-icons";

const TableTitle = props => {
  const { filterIsVisible, showFilter } = props;

  return (
    <div className="rates-table-title">
      <div
        className={filterIsVisible ? "FilterCell FilterShow" : "FilterCell"}
        onClick={() => showFilter()}
      >
        <div>
          <FontAwesomeIcon icon={faFilter} />
        </div>
      </div>
      <div className="Rate-cell1">Rate 1</div>
      <div className="Rate-cell2">Rate 2</div>
      <div className="Rate-cell3">Value</div>
      <div className="Rate-cell4">Trend</div>
    </div>
  );
};

export default TableTitle;
