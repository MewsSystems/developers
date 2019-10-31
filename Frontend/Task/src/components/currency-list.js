import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';

import { getConfiguration, getData } from '../actions';

class CurrencyList extends Component {
  componentDidMount() {
    const { dispatch } = this.props;
    dispatch(getConfiguration());
    setInterval(() => dispatch(getData()), 7500);
  }

  render() {
    const { configuration, data, searchText } = this.props;

    const filterX = searchText.toUpperCase();
    let displayClass = '';
    let noResultPre = true;
    let noResult = '';

    const loading =
      Object.entries(configuration).length === 0 &&
      React.createElement('h5', { className: 'mt-4 ml-5' }, 'Loading...');

    const currencyCouples = Object.keys(configuration).map(key => {
      const currencyCoupleLabel = `${configuration[key][0].name} / ${configuration[key][1].name}`;
      let colorStatus;
      let trendIcon;

      if (currencyCoupleLabel.toUpperCase().indexOf(filterX) > -1) {
        displayClass = '';
        noResultPre = false;
      } else {
        displayClass = 'displayNone';
      }

      noResult =
        noResultPre &&
        React.createElement('h5', { className: 'mt-4 ml-5' }, 'No Results');

      if (data[key]) {
        colorStatus = data[key]
          ? data[key].status === 200
            ? 'text-success'
            : 'text-danger'
          : '';

        trendIcon = {
          UP: 'fas fa-arrow-up',
          EQUAL: 'fas fa-equals',
          DOWN: 'fas fa-arrow-down',
          'N/A': 'fas fa-question',
        };

        trendIcon = React.createElement(
          'i',
          { className: trendIcon[data[key].trend] },
          ''
        );
      }

      return (
        <div className={`${displayClass} row bg-white d-flex my-1`} key={key}>
          <div className="col-6 align-self-center my-4">
            <h6 className="text-center m-0">{currencyCoupleLabel}</h6>
          </div>
          <div className="col-2 align-self-center">
            <p className="text-center m-0">{data[key] && data[key].rate}</p>
          </div>
          <div className={`col-2 align-self-center ${colorStatus}`}>
            <p className="text-center mb-1">{data[key] && data[key].status}</p>
            <p className="text-center mb-1">
              {data[key] && `(${data[key].statusText})`}
            </p>
          </div>
          <div className="col-2 align-self-center">
            {data[key] && (
              <div className="d-flex justify-content-center">{trendIcon}</div>
            )}
          </div>
        </div>
      );
    });
    return (
      <div className="col-12 app-list p-0">
        {loading}
        {noResult}
        {!loading && !noResult && (
          <div className="row bg-secondary">
            <div className="col-6 mt-2">
              <h6 className="text-center text-white">Currency Couples</h6>
            </div>
            <div className="col-2 mt-2">
              <h6 className="text-center text-white">Rates</h6>
            </div>
            <div className="col-2 mt-2">
              <h6 className="text-center text-white">Server Status</h6>
            </div>
            <div className="col-2 mt-2">
              <h6 className="text-center text-white">Trend</h6>
            </div>
          </div>
        )}
        {currencyCouples}
      </div>
    );
  }
}

CurrencyList.propTypes = {
  dispatch: PropTypes.func.isRequired,
  configuration: PropTypes.objectOf(PropTypes.array),
  data: PropTypes.objectOf(PropTypes.object),
  searchText: PropTypes.string,
};

CurrencyList.defaultProps = {
  configuration: {},
  data: {},
  searchText: '',
};

function mapStateToProps(state) {
  return {
    configuration: state.configuration,
    data: state.data,
    searchText: state.searchText,
  };
}

export default connect(mapStateToProps)(CurrencyList);
