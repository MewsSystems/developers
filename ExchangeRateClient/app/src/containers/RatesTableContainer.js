import {connect} from 'react-redux';
import {bindActionCreators} from 'redux';
import {actions} from '../actions/configActions';
import PropTypes from 'prop-types';
import RatesTable from '../components/RatesTable'
import React from 'react';
import { interval } from '../../config'

class RatesTableContainer extends React.Component {
  componentDidMount() {
    setInterval(this.props.actions.fetchRates, interval)
  }

  render() {
    const {
      config,
    } = this.props
    return (<RatesTable pairs={config} />)
  }
}

function mapStateToProps(state) { 
  const {
    config,
  } = state.config;
  return {
    config,
  }
}

function mapDispatchToProps(dispatch) {
  return {
    actions: bindActionCreators(actions, dispatch)
  };
}

export default connect(
  mapStateToProps,
  mapDispatchToProps,
)(RatesTableContainer);