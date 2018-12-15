import React, { Component } from "react";
import { connect } from "react-redux";
import {
  getConfig,
  updateActiveCurrencyPairs
} from "../../redux/modules/currency";

import RatesFilter from "./RatesFilter";

class RatesFilterContainer extends Component {
  componentDidMount() {
    this.props.getConfig();
  }

  render() {
    if (this.props.currencyPairs) {
      return (
        <RatesFilter
          currencyPairs={this.props.currencyPairs}
          activeCurrencyPairs={this.props.activeCurrencyPairs}
          updateActiveCurrencyPairs={this.props.updateActiveCurrencyPairs}
        />
      );
    }
    return null;
  }
}

const mapStateToProps = ({ currency }) => {
  const { currencyPairs, activeCurrencyPairs } = currency;

  return { currencyPairs, activeCurrencyPairs };
};

export default connect(
  mapStateToProps,
  {
    getConfig,
    updateActiveCurrencyPairs
  }
)(RatesFilterContainer);
