import React, { Component } from 'react'

import { connect } from 'react-redux';
import { Dispatch } from 'redux';


import { Actions as AlertActions } from '@store/reducers/alert.reducer';
import { ApplicationState } from '@store/types';


import ExchangeRates from '../containers/ExchangeRates';
import CenterContainer from './ui/CenterContainer';
import { ErrorBoundary } from './ui/ErrorBoundary';
import { AlertMessages, AlertMessagesProps } from './ui/AlertMessages';

interface PropsFromState {
    alert: AlertMessagesProps
}
interface PropsFromDispatch {
    hideAlert: typeof AlertActions.hideAlert
}

type MainPageProps = PropsFromState & PropsFromDispatch;

class MainPage extends Component<MainPageProps, {}> {
    render() {
        const { alert, hideAlert } = this.props;

        return (
            <CenterContainer>
                <ErrorBoundary>
                    <h1>Live Exchange Rates</h1>
                    <ExchangeRates />
                    <AlertMessages {...alert} onHide={hideAlert} />
                </ErrorBoundary>
            </CenterContainer>
        );
    }
}

const mapStateToProps = ({ alert }: ApplicationState) => ({
    alert: alert
} as PropsFromState)

const mapDispatchToProps = (dispatch: Dispatch) => ({
    hideAlert: () => dispatch(AlertActions.hideAlert()),
} as PropsFromDispatch)

export default connect(
    mapStateToProps,
    mapDispatchToProps
)(MainPage)