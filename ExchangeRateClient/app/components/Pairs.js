import React, { Component } from "react";
import { connect } from "react-redux";
import { endpoint } from "../config";
import { getPairs } from "../actions/currencyPairsAction";
import PropTypes from "prop-types";

class Pairs extends Component {
  componentDidMount() {
    this.props.getPairs();
  }
  render() {
    const { currencyPairs } = this.props;
    console.log(currencyPairs);
    return (
      <div>
        <h1>pairs component!</h1>
      </div>
    );
  }
}

Pairs.propTypes = {
  currencyPairs: PropTypes.array.isRequired
};

const mapStateToProps = state => ({
  currencyPairs: state.pairs.currencyPairs
});

export default connect(
  mapStateToProps,
  { getPairs }
)(Pairs);
