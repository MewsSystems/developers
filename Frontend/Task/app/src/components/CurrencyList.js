import React from 'react';
import { connect } from 'react-redux';
import _ from 'lodash';
import PropTypes from 'prop-types';
import { fetchCurrencies, fetchRates } from '../actions';
import Pair from './Pair';
import Spinner from './Spinner';

class CurrencyList extends React.Component {
  componentDidMount() {
    const { fetchCurrencies } = this.props;
    fetchCurrencies();
  }


  componentWillReceiveProps(nextProps, nextContext) {
    const { currencies, fetchRates } = this.props;

    if (!_.isEqual(currencies, nextProps.currencies)) {
      if (nextProps.currencies) {
        fetchRates(Object.keys(nextProps.currencies));
        setInterval(() => {
          fetchRates(Object.keys(nextProps.currencies));
        }, 2000);
      }
    }
  }

  getRateById = (uid) => {
    const { allRates } = this.props;
    if (!allRates) {
      return null;
    }

    return allRates[uid];
  };

  isPairValid = pair => ((!this.props.filteredValue) ? true : `${pair[0].code}/${pair[1].code}`.toLowerCase()
    .includes(this.props.filteredValue.toLowerCase()));

  renderPairs = () => {
    const { currencies } = this.props;
    const renderPairs = [];

    Object.keys(currencies)
      .forEach((pair) => {
        if (Object.prototype.hasOwnProperty.call(currencies, pair)) {
          const pairArr = currencies[pair];
          if (this.isPairValid(pairArr)) {
            renderPairs.push(
              <Pair
                key={pair}
                pairs={pairArr}
                rate={this.getRateById(pair)}
              />,
            );
          }
        }
      });

    return renderPairs;
  };

  render() {
    const { currencies } = this.props;

    if (_.isEmpty(currencies)) {
      return <Spinner />;
    }

    return (
      <div>
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
      </div>
    );
  }
}

/* for eslint */
CurrencyList.propTypes = {
  fetchCurrencies: PropTypes.func.isRequired,
  currencies: PropTypes.shape({ root: PropTypes.object }),
  fetchRates: PropTypes.func.isRequired,
  allRates: PropTypes.objectOf(PropTypes.number),
  filteredValue: PropTypes.string,
};

CurrencyList.defaultProps = {
  currencies: {},
  allRates: {},
  filteredValue: '',
};

const mapStateToProps = state => ({
  currencies: state.currencies.currencies,
  allRates: state.rates.rates,
  filteredValue: state.currencies.filteredValue,
});
export default connect(mapStateToProps, {
  fetchCurrencies,
  fetchRates,
})(CurrencyList);
