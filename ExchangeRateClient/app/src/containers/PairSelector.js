import {connect} from 'react-redux';
import {bindActionCreators} from 'redux';
import {actions} from '../actions/configActions';
import PropTypes from 'prop-types';
import React from 'react';
import Multiselect from '../components/Multiselect'

class PairSelector extends React.Component {
  
  async componentDidMount() {
    await this.props.actions.fetchConfig();
  }
  
  render() {
    const {
      config,
      isConfigFetching,
      actions,
    } = this.props;
    return (
      <Multiselect pairs={config} isConfigFetching={isConfigFetching} onPairToggle={actions.pairToggle}/>
    );
  }
}

function mapStateToProps(state) { 
  const {
    config,
    isConfigFetching,
  } = state.config;
  return {
    config,
    isConfigFetching,
  }
}

function mapDispatchToProps(dispatch) {
  return {
    actions: bindActionCreators(actions, dispatch)
  };
}

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(PairSelector);