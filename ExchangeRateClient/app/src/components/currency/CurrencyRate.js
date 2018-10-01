import React, { Component } from 'react'

let CurrencyRateCode = "";
export default class CurrencyRate extends Component {
  render() {
    CurrencyRateCode = this.props.currencyPairs[this.props.rateKey];
    return (
      <div className="rate">
        {(CurrencyRateCode) ?
          CurrencyRateCode[0].name + " / " + CurrencyRateCode[1].name + " is " + this.props.trend + ": " + this.props.rate[this.props.rateKey]
          : ""}
      </div>
    )
  }
}
