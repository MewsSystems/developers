import React from "react";
import "./styles.module.css";
import { loadState, namesArray } from "../../utils";

const renderOptions = namesArray.map(cur => {
  return (
    <option
      selected={cur.value === loadState("select")}
      key={cur.value}
      value={cur.value}
    >
      {cur.label}
    </option>
  );
});

type Props = {
  handleChange: Function;
  value: string;
};

const Select: React.FC<Props> = ({ handleChange, value, ...otherProps }) => {
  return (
    <select
      className="select"
      value={value}
      onChange={e => handleChange(e)}
      {...otherProps}
    >
      {renderOptions}
    </select>
  );
};

export default Select;
