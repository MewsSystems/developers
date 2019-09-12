import React from 'react';
import { connect } from 'react-redux';
import { Dispatch } from 'redux';

import { ApplicationState, ConnectedReduxProps, KeyByCurrencyPair } from '../store/types';
import { Actions as ConfigurationActions } from '../store/reducers/currencyPairs.reducer';
import CurrencyPairsTable from './CurrencyPairsTable';

interface PropsFromState {
    loading: boolean,
    currencyPairs: KeyByCurrencyPair[],
    currencyPairsIdList: string[]
}

interface PropsFromDispatch {
    fetchConfiguration: typeof ConfigurationActions.fetchConfiguration
}

type AllProps = PropsFromState & PropsFromDispatch;

class ExchangeRates extends React.Component<AllProps, {}> {
    constructor(props: AllProps) {
        super(props);
    }

    componentDidMount() {
        this.props.fetchConfiguration();
    }

    public render() {
        const { loading, currencyPairs, currencyPairsIdList } = this.props;

        return (
            <React.Fragment>
                <div>Eddy</div>
                {loading
                    ? <div>Loading</div>
                    : <CurrencyPairsTable currencyPairs={currencyPairs} currencyPairsIdList={currencyPairsIdList} />
                }
            </React.Fragment>
        )
    }
}

const mapStateToProps = ({ configuration }: ApplicationState) => ({
    loading: configuration.loading,
    currencyPairs: configuration.currencyPairs,
    currencyPairsIdList: configuration.currencyPairsIdList
} as PropsFromState);


const mapDispatchToProps = (dispatch: Dispatch) => ({
    fetchConfiguration: () => dispatch(ConfigurationActions.fetchConfiguration()),
} as PropsFromDispatch);

export default connect(
    mapStateToProps,
    mapDispatchToProps
)(ExchangeRates)