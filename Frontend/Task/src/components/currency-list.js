import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';

import { getConfiguration, getData } from '../actions';

class CurrencyList extends Component {
  componentDidMount() {
    const { dispatch, data } = this.props;
    // console.log('run-currencyList');
    dispatch(getConfiguration());
    setInterval(() => dispatch(getData()), 10000);
    // console.log('DATA', data);
  }

  render() {
    const { configuration, data, test3 } = this.props;
    // console.log('render2');

    if (data.length !== 0) {
      console.log('HERE', data);
    }

    if (Object.entries(configuration).length === 0) {
      return <div>Loading...</div>;
    }

    const currencyCouples = Object.keys(configuration).map(key => {
      let rate;

      if (Object.keys(data).length !== 0) {
        Object.keys(data).forEach(j => {
          // console.log('RATE-y', Object.keys(j).toString());
          // console.log('RATE-z', key);
          if (Object.keys(j).toString() === key) {
            // console.log('RATE-x', Object.entries(j)[0][1]);
            rate = Object.entries(j)[0][1];
          }
        });
      }

      return (
        <li className="list-group-item" key={key}>
          <p>
            Key: {key} {test3}
          </p>
          <p>
            Currency couple: {configuration[key][0].name} /{' '}
            {configuration[key][1].name}
          </p>
          <p>Rate: {rate}</p>
        </li>
      );
    });
    return <ul className="list-group">{currencyCouples}</ul>;
  }
}

CurrencyList.propTypes = {
  dispatch: PropTypes.func.isRequired,
  configuration: PropTypes.objectOf(PropTypes.array),
  data: PropTypes.objectOf(PropTypes.array),
  test3: PropTypes.string.isRequired,
};

CurrencyList.defaultProps = {
  configuration: {},
  data: {},
};

function mapStateToProps(state) {
  return {
    configuration: state.configuration,
    data: state.data,
    test3: state.test,
  };
}

export default connect(mapStateToProps)(CurrencyList);
