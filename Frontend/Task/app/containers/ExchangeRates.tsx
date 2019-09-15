import React from 'react';
import { connect } from 'react-redux';
import { Dispatch } from 'redux';

import { ApplicationState } from '@store/types';
import { Actions as CurrencyPairsActions } from '@store/reducers/currencyPairs.reducer';
import RatesTable from '@components/RatesTable';
import { ExchangeRatesProps, PropsFromState, PropsFromDispatch } from './types';
import CenterContainer from '@components/ui/CenterContainer';

class ExchangeRates extends React.Component<ExchangeRatesProps, {}> {
    constructor(props: ExchangeRatesProps) {
        super(props);
    }

    componentDidMount() {
        this.props.fetchCurrencyPairs();
    }

    public render() {
        const { loading, currencyPairs, currencyPairsIds, rates } = this.props;

        return (
            <CenterContainer>
                <h1>Live Exchange Rates</h1>
                {loading
                    ? <div>Loading</div>
                    : <RatesTable currencyPairs={currencyPairs} currencyPairsIdList={currencyPairsIds} rates={rates}/>
                }
            </CenterContainer>
        )
    }
}

const mapStateToProps = (state: ApplicationState) => ({
    loading: state.loading,
    currencyPairs: state.currencyPairs,
    rates: state.rates,
    currencyPairsIds: state.currencyPairsIds,
} as PropsFromState);


const mapDispatchToProps = (dispatch: Dispatch) => ({
    fetchCurrencyPairs: () => dispatch(CurrencyPairsActions.fetchCurrencyPairs()),
} as PropsFromDispatch);

export default connect(
    mapStateToProps,
    mapDispatchToProps
)(ExchangeRates)