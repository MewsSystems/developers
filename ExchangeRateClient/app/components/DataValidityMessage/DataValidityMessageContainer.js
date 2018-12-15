import React from "react";
import { connect } from "react-redux";

import DataValidityMessage from "./DataValidityMessage";

const DataValidityMessageContainer = props => {
  if (
    props.activeCurrencyPairs.length !== 0 &&
    props.currentRates &&
    props.updatedAt
  ) {
    return <DataValidityMessage time={props.updatedAt} />;
  }
  return null;
};

const mapStateToProps = ({ currency }) => {
  const { updatedAt, activeCurrencyPairs, currentRates } = currency;

  return { updatedAt, activeCurrencyPairs, currentRates };
};

export default connect(
  mapStateToProps,
  {}
)(DataValidityMessageContainer);
