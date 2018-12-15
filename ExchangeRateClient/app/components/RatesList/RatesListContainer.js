import React, { Component } from "react";
import { connect } from "react-redux";
import { getRates } from "../../redux/modules/currency";
import RatesList from "./RatesList";
import Spinner from "../Spinner";
import Message from "../Message";
import { REFRESH_INTERVAL } from "../../config";

class RatesListContainer extends Component {
  componentDidMount() {
    this.props.getRates();
    this.interval = setInterval(() => this.props.getRates(), REFRESH_INTERVAL);
  }

  componentWillUnmount() {
    clearInterval(this.interval);
  }

  render() {
    if (
      this.props.currencyPairs &&
      this.props.currentRates &&
      this.props.activeCurrencyPairs.length !== 0
    ) {
      const currencyPairs = this.props.activeCurrencyPairs.map(pairId => {
        const currencyPair = {};
        currencyPair.currency1 = this.props.currencyPairs[pairId][0];
        currencyPair.currency2 = this.props.currencyPairs[pairId][1];
        currencyPair.currentRate = this.props.currentRates[pairId];
        currencyPair.previousRate = this.props.previousRates
          ? this.props.previousRates[pairId]
          : null;
        currencyPair.id = pairId;
        return currencyPair;
      });
      return <RatesList currencyPairs={currencyPairs} />;
    } else if (
      this.props.currencyPairs &&
      this.props.currentRates &&
      this.props.activeCurrencyPairs.length === 0
    ) {
      return (
        <Message content="Please select currency pairs from the filter above." />
      );
    }
    return <Spinner />;
  }
}

const mapStateToProps = ({ currency }) => {
  const {
    activeCurrencyPairs,
    currencyPairs,
    currentRates,
    previousRates
  } = currency;

  return { activeCurrencyPairs, currencyPairs, currentRates, previousRates };
};

export default connect(
  mapStateToProps,
  {
    getRates
  }
)(RatesListContainer);
