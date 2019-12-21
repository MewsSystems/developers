import React from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCheck } from "@fortawesome/free-solid-svg-icons";

const DropdownFilter = props => {
  const { config, filter, changeFilter } = props;

  if (config) {
    return (
      <div>
        {Object.keys(config).map((item, i) => (
          <div
            key={item}
            className="filterItem"
            style={{ display: "flex", justifyContent: "center" }}
          >
            <div className="filterCheckBox" style={{ color: "#fd6722" }}>
              {filter[item] && <FontAwesomeIcon icon={faCheck} />}
            </div>
            <div
              className="filterIndex"
              style={{
                color: filter[item] ? "#ffa67d" : "rgba(253,103,34, 0.5)"
              }}
              onClick={() => changeFilter(item, "filter")}
            >
              {`${config[item][0].code}/${config[item][1].code}`}
            </div>
          </div>
        ))}
      </div>
    );
  }
};

export default DropdownFilter;
