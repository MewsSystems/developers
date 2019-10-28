import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';

import { getConfiguration, getData } from '../actions';

class CurrencyList extends Component {
  componentDidMount() {
    const { dispatch, data } = this.props;
    // console.log('run-currencyList');
    dispatch(getConfiguration());
    setInterval(() => dispatch(getData()), 7500);
    console.log('DATA', data);
  }

  render() {
    const { configuration, data, searchText } = this.props;
    // console.log('render2');

    const filterX = searchText.toUpperCase();
    let displayClass = '';
    let noResultPre = true;
    let noResult = '';

    const loading =
      Object.entries(configuration).length === 0
        ? React.createElement('div', { className: 'mt-4 ml-5' }, 'Loading...')
        : '';

    const currencyCouples = Object.keys(configuration).map(key => {
      const currencyCoupleLabel = `${configuration[key][0].name} / ${configuration[key][1].name}`;

      if (currencyCoupleLabel.toUpperCase().indexOf(filterX) > -1) {
        displayClass = '';
        noResultPre = false;
      } else {
        displayClass = 'displayNone';
      }
      noResult = noResultPre
        ? React.createElement('div', { className: 'mt-4 ml-5' }, 'No Results')
        : '';

      return (
        <li className={`list-group-item ${displayClass}`} key={key}>
          <p>Key: {key}</p>
          <p>Currency couple: {currencyCoupleLabel}</p>
          <p>Rate: {data[key] ? data[key].rate : ''}</p>
          <p>Status: {data[key] ? data[key].status : ''}</p>
          <p>StatusText: {data[key] ? data[key].statusText : ''}</p>
          <p>Trend: {data[key] ? data[key].trend : ''}</p>
        </li>
      );
    });
    return (
      <div className="col-md-12 app-list p-0">
        {loading}
        {noResult}
        <ul className="list-group">{currencyCouples}</ul>
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
