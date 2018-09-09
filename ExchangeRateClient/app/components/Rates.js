import React, { Component } from 'react';
import { connect } from 'react-redux';
import { updateRate } from '../middleware/api';
import PropTypes from 'prop-types';

class Rates extends Component {
  componentDidMount() {
    this.props.updateRate();
  }
  render() {
    const { currencyRates } = this.props;
    console.log(currencyRates);

    return <div>rates..something</div>;
  }
}

Rates.propTypes = {
  currencyRates: PropTypes.object.isRequired
};

const mapStateToProps = state => ({
  currencyRates: state.rates.currencyRates,
  loading: state.rates.loading,
  error: state.rates.error
});

export default connect(
  mapStateToProps,
  { updateRate }
)(Rates);
