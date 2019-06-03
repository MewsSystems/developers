import React, { PureComponent } from 'react';
import './App.scss';

import RatesList from '../RatesList';
import PairsSelector from '../PairsSelector'
import { fetchConfig, restoreConfig, fetchRate, setIntervalId } from '../../actions/currencies.js';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';

class App extends PureComponent {
  componentDidMount() {
    const { actions } = this.props;
  
    const select = window.localStorage.getItem('select');
    const config = window.localStorage.getItem('config');
    
    console.log('config', config);
    console.log('select', select);
    
    if (select && config) {
      const selectedValue = select !== 'undefined' ? JSON.parse(select) : undefined;
      const restoredConfig = JSON.parse(config);
      
      const { actions } = this.props;
      actions.restoreConfig(restoredConfig, selectedValue);
      if (selectedValue) {
        actions.setIntervalId(
          setInterval(() => {
            actions.fetchRate(selectedValue.map(x => x.value));
          }, 2000)
        );
      }
    } else {
      actions.fetchConfig();
    }
    
  }
  render() {
    const { loadingConfig } = this.props;
    return (
      <div className="react-root">
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
            <div>
            
            </div>
          </>
        }
      </div>
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

