import React from 'react';
import { connect } from 'react-redux';
import {Wrapper} from './CurrencyPairsFilter.styles';
import Loader from '../Loader/Loader';
import FilterCheckbox from './FilterCheckbox/FilterCheckbox'
import { TOGGLE_FILTER } from '../../store/actions';
import ICurrencyPairsFilter from '../../interfaces/CurrencyPairsFilter.interface'
import IRootState from '../../interfaces/InitialState.interface'
import { Dispatch } from 'redux';

const CurrencyPairsFilter: React.SFC<any> = (props : ICurrencyPairsFilter) => {
  const { configuration, filter, toggleFilter } = props;
  return (
    <Wrapper>
      <p>Currency Pairs Filter</p>
      <div className="Filters">
        {configuration.isLoading ? <Loader /> : null}
        {Object.keys(configuration.currencyPairs).map(key =>{
          return (
            <FilterCheckbox 
              currencyPair={configuration.currencyPairs[key]}
              toggleFilter={toggleFilter}
              filter={filter}
              id={key}
              key={key}
            />
          )
        })}
      </div>
    </Wrapper>
  );  
}

const mapStateToProps = (state: IRootState) => {
  const configuration = state.configuration;
  const filter = state.filter;
  return {
    configuration : configuration,
    filter : filter
  };
}

const mapDispatchToProps = (dispatch: Dispatch) => {
  return {
    toggleFilter: (pairsID: number, isChecked: boolean) => dispatch({type: TOGGLE_FILTER, payload : {pairsID: pairsID, isChecked: isChecked}})
  }
}

export default connect(mapStateToProps, mapDispatchToProps)(CurrencyPairsFilter);