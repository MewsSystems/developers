import React, {Component} from 'react';
import {connect} from 'react-redux';

import {toggleSelectedPair} from '../actions/pairsActions';

const Pair = ({id, pair, setPair, isSelected, pairCode}) => {
  return (
    <div style={{display: 'inline-block'}}>
      <span
        className={'pretty-button ' + (isSelected ? 'filled' : '')}
        onClick={setPair}
        style={{marginRight: 10, marginBottom: 10}}
      >
        {pairCode}
      </span>
    </div>
  );
};

class PairSelector extends Component {
  render () {
    const {currencyPairs, selectedPairs, toggleSelectedPair} = this.props;
    const selectedPairsCodes = selectedPairs.map (e => e.pairCode);

    return (
      <div className="">
        <h1>Currency pairs</h1>
        {currencyPairs.map (pair => (
          <Pair
            key={pair.id}
            {...pair}
            setPair={() => toggleSelectedPair (pair)}
            isSelected={selectedPairsCodes.indexOf (pair.pairCode) !== -1}
          />
        ))}
      </div>
    );
  }
}

const mapStateToProps = state => {
  return {
    currencyPairs: state.pairsReducer.currencyPairs,
    selectedPairs: state.pairsReducer.selectedPairs,
  };
};

const mapDispatchToProps = dispatch => {
  return {
    toggleSelectedPair: selectedPair => {
      dispatch (toggleSelectedPair (selectedPair));
    },
  };
};

export default connect (mapStateToProps, mapDispatchToProps) (PairSelector);
