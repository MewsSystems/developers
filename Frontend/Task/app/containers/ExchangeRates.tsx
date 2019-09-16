import 'url-search-params-polyfill';
import React from 'react';
import { connect } from 'react-redux';
import { Dispatch } from 'redux';
import { ApplicationState } from '@store/types';
import { Actions as CurrencyPairsActions } from '@store/reducers/currency-pairs.reducer';
import RatesTable from '@components/RatesTable/RatesTable';
import TableController from '@components/RatesTable/TableController';
import TableShell from '@components/ui/TableShell/TableShell.base';
import { initUrlParams } from '@utils/initUrlParams';
import { ExchangeRatesProps, PropsFromState, PropsFromDispatch, UrlParams } from './types';

interface ExchangeRatesState {
    urlParams: UrlParams,
    searchTerm: string
}

class ExchangeRates extends React.Component<ExchangeRatesProps, ExchangeRatesState> {
    constructor(props: ExchangeRatesProps) {
        super(props);

        const urlParams = initUrlParams();

        this.state = {
            searchTerm: urlParams.searchTerm || '',
            urlParams: urlParams
        }
    }

    componentDidMount() {
        this.props.fetchCurrencyPairs();
    }

    onSearch = (value: string) => {
        this.setState({
            searchTerm: value,
            urlParams: {
                searchTerm: value,
            }
        }, () => {
            this.updateUrlParams(value);
        });
    }

    updateUrlParams(value: string) {
        const { history } = this.props;

        let urlParams = new URLSearchParams(window.location.search);

        if (value === "") {
            urlParams.delete('q');
        } else {
            urlParams.set('q', value);
        }

        history.push("?" + urlParams.toString());
    }

    public render() {
        const { loading } = this.props;
        const { searchTerm, urlParams } = this.state;

        return (
            <>
                {loading
                    ? <TableShell  rows={10} cols={3} />
                    : (
                        <>
                            <TableController searchTerm={searchTerm} onSearch={this.onSearch}/>
                            <RatesTable urlParams={urlParams}/>
                        </>
                    )
                }
            </>
        )
    }
}

const mapStateToProps = ({currencyState}: ApplicationState) => ({
    loading: currencyState.loading,
} as PropsFromState);


const mapDispatchToProps = (dispatch: Dispatch) => ({
    fetchCurrencyPairs: () => dispatch(CurrencyPairsActions.fetchCurrencyPairs()),
} as PropsFromDispatch);

export default connect(
    mapStateToProps,
    mapDispatchToProps
)(ExchangeRates)