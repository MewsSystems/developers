
import React from 'react';
import { Router } from 'react-router-dom';
import { Provider } from 'react-redux';
import { Store } from 'redux';
import { History } from 'history';

import MainPage from '@components/MainPage';
import { ApplicationState } from './store/types';

interface ApplicationProps {
    store: Store<ApplicationState>,
    history: History
}

class Application extends React.Component<ApplicationProps> {
    public render() {
        const { store, history } = this.props;

        return (
            <Provider store={store}>
                <Router history={history}>
                    <MainPage />
                </Router>
            </Provider>
        )
    }
}

export default Application;