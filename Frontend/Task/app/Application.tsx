
import React from 'react';
import { Provider } from 'react-redux'
import { Store } from 'redux'

import { ApplicationState } from './store/types';
import ExchangeRates from './components/ExchangeRates';

interface PropsFromDispatch {
    [key: string]: any
}

interface OwnProps {
    store: Store<ApplicationState>
}

type AllProps = PropsFromDispatch & OwnProps;

class Application extends React.Component<AllProps> {
    public render() {
        const { store } = this.props;

        return (
            <Provider store={store}>
                <ExchangeRates />
            </Provider>
        )
    }
}

export default Application;