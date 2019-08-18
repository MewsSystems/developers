import React from 'react';
import { connect } from 'react-redux';
import {Wrapper} from './CurrencyPairsFilter.styles';
import Loader from '../Loader/Loader';
import FilterCheckbox from './FilterCheckbox/FilterCheckbox'
import { TOGGLE_FILTER } from '../../store/actions';

const CurrencyPairsFilter = (props) => {
    return (
      <Wrapper>
            <p>Currency Pairs Filter</p>
            <div className="Filters">
                {props.configuration.isLoading ? <Loader /> : null}
                {Object.keys(props.configuration.currencyPairs).map(key =>{
                    return (
                        <FilterCheckbox 
                            currencyPair={props.configuration.currencyPairs[key]}
                            toggleFilter={props.toggleFilter}
                            filter={props.filter}
                            id={key}
                            key={key}
                        />
                    )
                })}
            </div>
      </Wrapper>
    );  
}
const mapStateToProps = state => {
    const configuration = state.configuration;
    const filter = state.filter;
    return {
        configuration : configuration,
        filter : filter
    };
}
const mapDispatchToProps = dispatch => {
    return {
        toggleFilter: (pairsID, isChecked) => dispatch({type: TOGGLE_FILTER, payload : {pairsID: pairsID, isChecked: isChecked}})
    }
}
export default connect(mapStateToProps, mapDispatchToProps)(CurrencyPairsFilter);

