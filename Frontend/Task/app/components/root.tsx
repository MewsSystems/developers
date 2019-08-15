import * as React from 'react';
import { BrowserRouter, Route, Switch } from 'react-router-dom';
import MainPage from './pages/MainPage';

export default class Root extends React.Component<any, any> {
    public render() {
        return (
            <BrowserRouter>
                <Switch>
                    <Route exact={true} path="/" component={MainPage} />
                    <Route>
                        <h2>Page Not Found</h2>
                    </Route>
                </Switch>
            </BrowserRouter>
        );
    }
}