import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';

// components
import Rates from '../rates';
import Error from '../../components/error';
import LoadingSpinner from '../../components/loading-spinner';

// actions
import getInitialConfiguration from '../../actions/configuration';

/**
 * Root application container
 */
class Application extends Component {
  componentDidMount() {
    const {
      hasConfigurationData,
      getInitialConfiguration: getConfiguration,
    } = this.props;

    if (!hasConfigurationData) {
      getConfiguration();
    }
  }

  render() {
    const {
      isLoading,
      isError,
    } = this.props;

    if (isError) return <Error />;
    if (isLoading) return <LoadingSpinner />;

    return <Rates />;
  }
}

Application.defaultProps = {
  hasConfigurationData: false,
  isLoading: false,
  isError: false,
};

Application.propTypes = {
  getInitialConfiguration: PropTypes.func.isRequired,
  hasConfigurationData: PropTypes.bool,
  isLoading: PropTypes.bool,
  isError: PropTypes.bool,
};

/**
 * Mapping state values to container props
 * @param state
 */
const mapStateToProps = (state) => ({
  hasConfigurationData: !!state.configuration.currencyPairs,
  isLoading: state.configuration.isLoading,
  isError: state.configuration.isError,
});

const mapDispatchToProps = {
  getInitialConfiguration,
};

export default connect(mapStateToProps, mapDispatchToProps)(Application);
