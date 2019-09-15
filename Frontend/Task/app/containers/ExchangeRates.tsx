import React from 'react';
import { connect } from 'react-redux';
import { Dispatch } from 'redux';

import { ApplicationState } from '@store/types';
import { Actions as CurrencyPairsActions } from '@store/reducers/currency-pairs.reducer';

import RatesTable from '@components/RatesTable';
import TableController from '@components/TableController';
import TableShell from '@components/ui/TableShell/TableShell.base';

import { ExchangeRatesProps, PropsFromState, PropsFromDispatch } from './types';

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
            <>
                {loading
                    ? <TableShell  rows={10} cols={3} />
                    : (
                        <>
                            <TableController />
                            <RatesTable currencyPairs={currencyPairs} currencyPairsIdList={currencyPairsIds} rates={rates} />
                        </>
                    )
                }
            </>
        )
    }
}

const mapStateToProps = ({currencyState, ratesState}: ApplicationState) => ({
    loading: currencyState.loading,
    currencyPairs: currencyState.currencyPairs,
    currencyPairsIds: currencyState.currencyPairsIds,
    rates: ratesState.rates,
} as PropsFromState);


const mapDispatchToProps = (dispatch: Dispatch) => ({
    fetchCurrencyPairs: () => dispatch(CurrencyPairsActions.fetchCurrencyPairs()),
} as PropsFromDispatch);

export default connect(
    mapStateToProps,
    mapDispatchToProps
)(ExchangeRates)