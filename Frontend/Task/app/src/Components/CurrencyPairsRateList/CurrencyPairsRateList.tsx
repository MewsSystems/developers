import React from 'react';
import { connect } from 'react-redux';
import CurrencyPair from './CurrencyPair/CurrencyPair';
import CurrencyPairRate from './CurrencyPairRate/CurrencyPairRate';
import CurrencyPairStatus from './CurrencyPairStatus/CurrencyPairStatus';
import { Wrapper, CurrencyPairsWrapper } from './CurrencyPairsRateList.styles';
import Loader from '../Loader/Loader';
import IRootState from '../../interfaces/InitialState.interface'
import ICurrencyPairsRateList from '../../interfaces/CurrencyPairsRateList.interface';
import LoadingPlaceholder from './LoadingPlaceholder/LoadingPlaceholder'

const CurrencyPairsRateList: React.SFC<any> = (props : ICurrencyPairsRateList) => {
  const { configuration, filter, rates } = props;
  return (
    <Wrapper>
        <div className="RateListHead">
            <p>Currency</p>
            <p>Rate</p>
            <p>Status</p>
        </div>
        {configuration.isLoading ? <Loader /> : null}
        {Object.keys(configuration.currencyPairs).map(key =>{
          return (
            filter.filter(id => id === key).length === 0 ? 
              <CurrencyPairsWrapper key={key}>
                <CurrencyPair currencyPair={configuration.currencyPairs[key]}/>
                {!rates.isLoading ?
                  <>
                    <CurrencyPairRate rateValue={rates.currencyPairsRates[key].value}/>
                    <CurrencyPairStatus rateStatus={rates.currencyPairsRates[key].status}/>
                  </>
                : 
                  <>
                    <LoadingPlaceholder />
                    <LoadingPlaceholder />
                  </>
                }
              </CurrencyPairsWrapper>
            : null
          )
        })}
    </Wrapper>
  );
}

const mapStateToProps = (state: IRootState) => {
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
