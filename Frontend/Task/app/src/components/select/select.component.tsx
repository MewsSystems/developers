import React from "react";
import "./styles.module.css";
import { namesArray } from "../../utils";
import { FilterProps } from "../../redux/filter/filter.models";

const Select: React.FC<FilterProps> = ({ handleChange, value }) => {
  return (
    <select
      className="select"
      placeholder="select by name"
      value={value}
      onChange={handleChange}
    >
      {namesArray.map(cur => (
        <option value={cur.value}>{cur.label}</option>
      ))}
    </select>
  );
};

export default Select;
