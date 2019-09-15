
import React from 'react';
import { Provider } from 'react-redux'
import { Store } from 'redux'

import { ApplicationState } from './store/types';
import MainPage from '@components/MainPage';

interface ApplicationProps {
    store: Store<ApplicationState>
}

class Application extends React.Component<ApplicationProps> {
    public render() {
        const { store } = this.props;

        return (
            <Provider store={store}>
                <MainPage />
            </Provider>
        )
    }
}

export default Application;