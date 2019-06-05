import React, {PureComponent} from 'react';
import {connect} from 'react-redux';
import { bindActionCreators } from 'redux';
import Select from 'react-select';
import {
  fetchRate, clearData,
  setSelectValue, setIntervalId,
  setTimerIntervalId
} from '../../actions/currencies.js';
// import countDown from '../../lib/countDown';

import { TIMEOUT } from '../../constants/timeout.js';

class PairsSelector extends PureComponent {
  constructor() {
    super();
  
    this.onSelectChange = ::this.onSelectChange;
  }
  
  componentDidMount() {
  
  }
  
  onSelectChange(value) {
    const { actions, intervalId } = this.props;
    actions.setSelectValue(value);
    
    if (value?.length > 0) {
      clearInterval(intervalId);
      
      const ids = value.map(x => x.value);
      
      window.localStorage.setItem('select', JSON.stringify(value));
      
      actions.fetchRate(ids);
      actions.setIntervalId(
        setInterval(() => {
          actions.fetchRate(ids);
        }, TIMEOUT)
      );
    } else {
      clearInterval(intervalId);
      window.localStorage.setItem('select', '');
      actions.clearData();
    }
  }
  
  render() {
    const { selectedPair, options } = this.props;
    return (
      <div className='select--container'>
        <h2 className='select--header'>Currency pairs filter</h2>
        <Select
          options={options}
          value={selectedPair}
          onChange={this.onSelectChange}
          isMulti
          className='select'
          placeholder='Select a pair of currencies...'
        />
      </div>
    );
  }
}

const mapStateToProps = state => ({
  options: state.currencies.options,
  selectedPair: state.currencies.selectedPair,
  intervalId: state.currencies.intervalId,
  newIntervalId: state.currencies.newIntervalId
});

const mapDispatchToProps = dispatch => ({
  actions: bindActionCreators({
    fetchRate, clearData,
    setSelectValue, setIntervalId,
    setTimerIntervalId
  }, dispatch)
});

export default connect(mapStateToProps, mapDispatchToProps)(PairsSelector);
