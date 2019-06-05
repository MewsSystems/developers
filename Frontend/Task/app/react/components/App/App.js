import React, { PureComponent } from 'react';
import './App.scss';

import RatesList from '../RatesList';
import PairsSelector from '../PairsSelector';
import Message from '../Message';

import { fetchConfig, restoreConfig, fetchRate, setIntervalId } from '../../actions/currencies.js';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';

import { TIMEOUT } from '../../constants/timeout.js';

class App extends PureComponent {
  componentDidMount() {
    const { actions } = this.props;
  
    const select = window.localStorage.getItem('select');
    const config = window.localStorage.getItem('config');
    
    if (select) {
      const selectedValue = select !== 'undefined' ? JSON.parse(select) : undefined;
      const restoredConfig = JSON.parse(config);
      
      const { actions } = this.props;
      actions.restoreConfig(restoredConfig, selectedValue);
      
      if (selectedValue) {
        const ids = selectedValue.map(x => x.value);
        actions.fetchRate(ids);
        actions.setIntervalId(
          setInterval(() => {
            actions.fetchRate(ids);
          }, TIMEOUT)
        );
      }
    } else {
      actions.fetchConfig();
    }
    
  }
  render() {
    const { loadingConfig } = this.props;
    return (
      <>
        {
          loadingConfig &&
            <div className='spinner--container'>
              <div className="spinner" />
            </div>
        }
        {
          !loadingConfig &&
          <>
            <PairsSelector/>
            <RatesList/>
            <Message />
          </>
        }
      </>
    );
  }
}

const mapStateToProps = state => ({
  loadingConfig: state.currencies.loadingConfig,
});

const mapDispatchToProps = dispatch => ({
  actions: bindActionCreators({ fetchConfig, restoreConfig, fetchRate, setIntervalId }, dispatch)
});

export default connect(mapStateToProps, mapDispatchToProps)(App);

