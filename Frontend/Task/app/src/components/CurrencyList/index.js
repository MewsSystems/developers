import React from 'react';
import { connect } from 'react-redux';
import _ from 'lodash';
import PropTypes from 'prop-types';
import { fetchCurrencies, fetchRates } from '../../actions';
import PairRow from '../PairRow';
import Spinner from '../Spinner';
import './styles.css';

class CurrencyList extends React.Component {
  componentDidMount() {
    const { callFetchCurrencies } = this.props;
    callFetchCurrencies();
    setInterval(() => {
      this.callForRates();
    }, 2000);
  }

  callForRates = () => {
    const { callFetchRates, currencies } = this.props;
    if (currencies) {
      callFetchRates(Object.keys(currencies));
    }
  };

  getRateById = (rates, uid) => {
    if (!rates) {
      return null;
    }

    return rates[uid];
  };

  isPairValid = (filterString, pair) => ((!filterString) ? true : pair.toLowerCase()
    .includes(filterString.toLowerCase()));

  renderPairs = () => {
    const {
      currencies, filteredValue, allRates, allOldRates,
    } = this.props;
    const renderPairs = [];

    Object.keys(currencies)
      .forEach((pairKey) => {
        if (Object.prototype.hasOwnProperty.call(currencies, pairKey)) {
          const [pairOne, pairTwo] = currencies[pairKey];
          const pairString = `${pairOne.code}/${pairTwo.code}`;
          if (this.isPairValid(filteredValue, pairString)) {
            renderPairs.push(
              <PairRow
                key={pairKey}
                pair={pairString}
                rate={this.getRateById(allRates, pairKey)}
                oldRate={this.getRateById(allOldRates, pairKey)}
              />,
            );
          }
        }
      });

    return renderPairs;
  };

  getContentOfTable = () => {
    const pairsBody = this.renderPairs();
    if (_.isEmpty(pairsBody)) {
      return (
        <tr>
          <td colSpan="3">
            <div className="alert alert-warning">Currency pairs not found, try change your filter value</div>
          </td>
        </tr>
      );
    }
    return pairsBody;
  };

  render() {
    const { currencies } = this.props;

    const pairsBody = this.getContentOfTable();

    if (_.isEmpty(currencies)) {
      return <Spinner />;
    }

    return (
      <div>
        <table className="table table-striped">
          <thead className="thead-dark">
            <tr>
              <th>Currency pair</th>
              <th>Actual course</th>
              <th className="text-right">Trend</th>
            </tr>
          </thead>
          <tbody>
            {pairsBody}
          </tbody>
        </table>
      </div>
    );
  }
}

/* for eslint */
CurrencyList.propTypes = {
  callFetchCurrencies: PropTypes.func.isRequired,
  currencies: PropTypes.shape({ root: PropTypes.object }),
  callFetchRates: PropTypes.func.isRequired,
  allRates: PropTypes.objectOf(PropTypes.number),
  allOldRates: PropTypes.objectOf(PropTypes.number),
  filteredValue: PropTypes.string,
};

CurrencyList.defaultProps = {
  currencies: {},
  allRates: {},
  allOldRates: {},
  filteredValue: '',
};

const mapStateToProps = state => ({
  currencies: state.currencies.currencies,
  allRates: state.rates.rates,
  allOldRates: state.rates.oldRates,
  filteredValue: state.currencies.filteredValue,
});
export default connect(mapStateToProps, {
  callFetchCurrencies: fetchCurrencies,
  callFetchRates: fetchRates,
})(CurrencyList);
