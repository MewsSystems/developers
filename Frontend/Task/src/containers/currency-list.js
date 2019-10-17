import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';

import { getConfiguration, test } from '../actions';

class CurrencyList extends Component {
  componentDidMount() {
    const { dispatch } = this.props;
    console.log('run-currencyList');
    dispatch(test);
    dispatch(getConfiguration()); // run & break
  }

  render() {
    const { configuration, test3 } = this.props;

    console.log('render2', configuration);
    if (Object.entries(configuration).length === 0) {
      console.log('here');
      return <div>Loading...{test3}</div>;
    }
    console.log('render');
    const currencyCouples = Object.keys(configuration).map(key => (
      <li className="list-group-item" key={key[0]}>
        <p>Key: {key[0]}</p>
        <p>
          Rate: {key[1][0].name} / {key[1][1].name}
        </p>
      </li>
    ));
    return <ul className="list-group">{currencyCouples}</ul>;
  }
}

CurrencyList.propTypes = {
  dispatch: PropTypes.func.isRequired,
  // eslint-disable-next-line react/forbid-prop-types
  configuration: PropTypes.object,
  test3: PropTypes.string.isRequired,
};

CurrencyList.defaultProps = {
  configuration: '',
};

function mapStateToProps(state) {
  return {
    configuration: state.configuration,
    test3: state.test,
  };
}

// function mapDispatchToProps(dispatch) {
//   return bindActionCreators(
//     {
//       configurationLoader: getConfiguration,
//       test2: test,
//     },
//     dispatch
//   );
// }

export default connect(
  mapStateToProps
  // mapDispatchToProps
)(CurrencyList);
