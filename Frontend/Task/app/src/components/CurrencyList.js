import React, { useEffect } from 'react';
import { connect } from 'react-redux';
import _ from 'lodash';
import { fetchCurrencies, fetchRates } from '../actions';
import Pair from './Pair';

class CurrencyList extends React.Component {
  componentDidMount() {
    this.props.fetchCurrencies();
  }


  componentWillReceiveProps(nextProps, nextContext) {
    if (!_.isEqual(this.props.currencies, nextProps.currencies)) {
      if (nextProps.currencies) {
        this.props.fetchRates(Object.keys(nextProps.currencies));
        setInterval(() => {
          this.props.fetchRates(Object.keys(this.props.currencies));
        }, 500);
      }
    }
  }

  getRateById = (uid) => {
    if (!this.props.allRates) {
      return null;
    }

    return this.props.allRates[uid];
  };


  renderPairs = () => {
    const renderPairs = [];

    for (const pair in this.props.currencies) {
      if (this.props.currencies.hasOwnProperty(pair)) {
        renderPairs.push(
          <Pair
            key={pair}
            pairs={this.props.currencies[pair]}
            rate={this.getRateById(pair)}
          />,
        );
      }
    }

    return renderPairs;
  };

  render() {
    if (_.isEmpty(this.props.currencies)) {
      return <div>Loading Currencies...</div>;
    }

    return (
      <table className="table">
        <thead>
          <tr>
            <th>Currency pair</th>
            <th>Actual course</th>
            <th>Trend</th>
          </tr>
        </thead>
        <tbody>
          {this.renderPairs()}
        </tbody>
      </table>
    );
  }
}

const mapStateToProps = state => ({
  currencies: state.currencies,
  allRates: state.rates.rates,
});
export default connect(mapStateToProps, { fetchCurrencies, fetchRates })(CurrencyList);
