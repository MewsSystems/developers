import React from 'react';
import { connect } from 'react-redux';
import { Dispatch } from 'redux';


import { ApplicationState, ConnectedReduxProps, CurrencyPair } from '../store/types';
import { Actions as ConfigurationActions } from '../store/reducers/currencyPairs.reducer';
import CurrencyPairsTable from './CurrencyPairsTable';


interface PropsFromState {
    loading: boolean,
    currencyPairs: CurrencyPair[]
}

interface PropsFromDispatch {
    fetchConfiguration: typeof ConfigurationActions.fetchConfiguration
}

type AllProps = PropsFromState & PropsFromDispatch;

class ExchangeRates extends React.Component<AllProps, {}> {
    constructor(props: AllProps) {
        super(props);

        this.state = {
            extenstionModalMinDate: '',
            showExtenstionModal: false,
        }
    }

    componentDidMount() {
        this.props.fetchConfiguration();
    }

    public render() {
        const { loading, currencyPairs } = this.props;

        return (
            <React.Fragment>
                {loading
                    ? <div>Loading</div>
                    : <CurrencyPairsTable currencyPairs={currencyPairs} />
                }
            </React.Fragment>
        )
    }
}

const mapStateToProps = ({ configuration }: ApplicationState) => ({
    loading: configuration.loading,
    currencyPairs: configuration.currencyPairs
} as PropsFromState);


const mapDispatchToProps = (dispatch: Dispatch) => ({
    fetchConfiguration: () => dispatch(ConfigurationActions.fetchConfiguration()),
} as PropsFromDispatch);

export default connect(
    mapStateToProps,
    mapDispatchToProps
)(ExchangeRates)