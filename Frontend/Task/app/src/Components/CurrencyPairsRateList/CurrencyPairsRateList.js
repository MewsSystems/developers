import React from 'react';
import { connect } from 'react-redux';
import CurrencyPair from './CurrencyPair/CurrencyPair';
import CurrencyPairRate from './CurrencyPairRate/CurrencyPairRate';
import CurrencyPairStatus from './CurrencyPairStatus/CurrencyPairStatus';
import { Wrapper, CurrencyPairsWrapper } from './CurrencyPairsRateList.styles';
import Loader from '../Loader/Loader';

const CurrencyPairsRateList = (props) => {
    return (
      <Wrapper>
          <div className="RateListHead">
              <p>Currency</p>
              <p>Rate</p>
              <p>Status</p>
          </div>
          {props.configuration.isLoading ? <Loader /> : null}
          {Object.keys(props.configuration.currencyPairs).map(key =>{
            return (
              props.filter.filter(id => id === key).length === 0 ? 
              <CurrencyPairsWrapper key={key}>
                <CurrencyPair currencyPair={props.configuration.currencyPairs[key]}/>
                {!props.rates.isLoading ?
                    <>
                        <CurrencyPairRate rateValue={props.rates.currencyPairsRates[key].value}/>
                        <CurrencyPairStatus rateStatus={props.rates.currencyPairsRates[key].status}/>
                    </>
                : null}
              </CurrencyPairsWrapper>
              : null
            )
          })}
      </Wrapper>
    );
}

const mapStateToProps = state => {
  const configuration = state.configuration;
  const rates = state.rates;
  const filter = state.filter;
  return {
      configuration : configuration,
      rates : rates,
      filter : filter
  };
}

export default connect(mapStateToProps)(CurrencyPairsRateList);
