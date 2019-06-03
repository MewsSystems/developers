import React, {PureComponent} from 'react';
import {connect} from 'react-redux';
import { bindActionCreators } from 'redux';
import Select from 'react-select';
import { fetchRate, clearData, setSelectValue, setIntervalId } from '../../actions/currencies.js';

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
      
      window.localStorage.setItem('select', JSON.stringify(value));
      
      actions.setIntervalId(
        setInterval(() => {
          actions.fetchRate(value.map(x => x.value));
        }, 2000)
      );
    } else {
      clearInterval(intervalId);
      window.localStorage.setItem('select', '');
      // window.localStorage.removeItem('select');
      // window.localStorage.clear();
      actions.clearData();
    }
  }
  
  render() {
    const { selectedPair, options } = this.props;
    return (
      <div>
        <Select
          options={options}
          value={selectedPair}
          onChange={this.onSelectChange}
          isMulti
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
  actions: bindActionCreators({ fetchRate, clearData, setSelectValue, setIntervalId }, dispatch)
});

export default connect(mapStateToProps, mapDispatchToProps)(PairsSelector);
