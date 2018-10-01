import React, { Component } from 'react';
import { PropTypes } from 'prop-types';
import { connect } from 'react-redux';
import { Spinner } from '../common';
import { getConfiguration, getRates } from '../../actions';
import CurrencyRate from '../currency/CurrencyRate'
import CurrenctSelect from '../currency/CurrencySelect';
let pair = "";
let Trend = "Stagnating";
let configuration=""
class Landing extends Component {
  constructor() {
    super()
    this.handleInput = this.handleInput.bind(this)
  }
  componentDidMount() {
    // get the selected currency Pairs
    pair = localStorage.getItem('pairValue');
    configuration = JSON.parse(localStorage.getItem("configuration"));
    this.props.getConfiguration();
    if (pair) {
      this.props.getRates(pair);
    }
  }
  //handle in change input
  handleInput(pair) {
    localStorage.setItem('pairValue', pair);
    this.props.getRates(pair);
    
  }

  componentWillReceiveProps(nextProps) {

    if(nextProps.configuration.length !=0){
    localStorage.setItem("configuration", JSON.stringify(nextProps.configuration));
    configuration = JSON.parse(localStorage.getItem("configuration"));
    }
    if (nextProps.rateKey == this.props.rateKey) {
      if (nextProps.rates[nextProps.rateKey] > this.props.rates[this.props.rateKey]) {
        Trend = 'Growing'
      } else if (nextProps.rates[nextProps.rateKey] < this.props.rates[this.props.rateKey]) {
        Trend = 'Declining'
      } else if (nextProps.rates[nextProps.rateKey] == this.props.rates[this.props.rateKey]) {
        Trend = 'Stagnating'
      }
    }
  }
  render() {
    // update the rate every 30 sec
    pair = localStorage.getItem('pairValue');
    if (pair) {
      setTimeout(function () {
        this.props.getRates(pair);
      }.bind(this), 30000);
    }
    let mainContent;
    if (configuration.length == 0) {
      mainContent =  <div className="body"><Spinner /></div>;

    } else {
      let pairValue = localStorage.getItem('pairValue');
      mainContent =
        <div className="body">
          <CurrenctSelect
            value={(pairValue) ? pairValue : "_none"}
            handleChange={this.handleInput}
            currencyPairs={configuration}
          />
          <CurrencyRate trend={Trend} currencyPairs={configuration} rateKey={this.props.rateKey} rate={this.props.rates} />
        </div>
    }
    return (
      <div className="landing">
        {mainContent}
      </div>
    )
  }
}
const mapStateToProps = ({ currency }) => {
  const { configuration, rates, rateKey } = currency;
  return { configuration, rates, rateKey };
}

export default connect(mapStateToProps, { getRates, getConfiguration })(
  Landing
);
