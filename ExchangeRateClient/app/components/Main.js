import React, { Component } from 'react';
import Pairs from './../components/Pairs';

// import { connect } from 'react-redux';
// import { updateRate } from '../middleware/api';
// import PropTypes from 'prop-types';

class Main extends Component {
  // componentDidMount() {
  //   this.props.updateRate();
  // }
  render() {
    // const { currencyRates } = this.props;

    return (
      <div>
        <Pairs />
      </div>
    );
  }
}

// Main.propTypes = {
//   currencyRates: PropTypes.object.isRequired
// };

// const mapStateToProps = state => ({
//   currencyRates: state.pairs.currencyRates,
//   loading: state.pairs.loading,
//   error: state.pairs.error
// });

export default Main;
