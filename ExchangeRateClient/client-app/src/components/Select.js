// @flow strict

import * as React from "react";
import Select from "react-select";
import { connect } from "react-redux";
import config from "../records/config";
import { fetchRatesAction, selectIds } from "../actions/configActions";

import createRateLabel from "../utils/createRateLabel";

const customStyles = {
  option: (base, state) => ({
    ...base,
    backgroundColor: state.isFocused ? "rgba(249, 222, 201, 1)" : base.backgroundColor,
    color: "rgba(58, 64, 90, 1)",
  }),
  control: (base, state) => ({
    ...base,
    backgroundColor: "transparent",
    boxShadow: "inset -11px -15px 84px -32px rgba(0,0,0,1)",
  }),
  multiValue: (base, state) => {
    const opacity = state.isDisabled ? 0.5 : 1;
    const transition = "opacity 300ms";
    return { ...base, opacity, transition, fontSize: "1.5em" };
  },
};

const CustomSelect = props => {
  if (props.data.length === 0) return null;
  const { data, selectedRates, fetchRatesAction, selectIds } = props;

  const opt = Object.keys(data).reduce((acc, id) => {
    const item = {
      value: id,
      label: createRateLabel(data[id].currencies),
    };

    return [...acc, item];
  }, []);

  return (
    <Select
      styles={customStyles}
      isMulti
      onChange={data => selectIds(data.map(({ value }) => value))}
      options={opt}
      defaultValue={opt.filter(item => selectedRates.includes(item.value))}
    />
  );
};

const mapStateToProps = ({ data, selectedRates = [] }) => ({
  data,
  selectedRates,
});

export default connect(
  mapStateToProps,
  { fetchRatesAction, selectIds },
)(CustomSelect);
