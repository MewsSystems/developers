import React from 'react';
import { Route, Switch, Redirect } from 'react-router-dom';
import MoviePage from './pages/MoviePage';
import SearchPage from './pages/SearchPage';

interface Props {
    className?: string,
}

export default function Routing({ className }: Props) {
    return (
        <div className={className}>
            <Switch>
                <Route path={'/movie/:id'}>
                    <MoviePage />
                </Route>
                <Route path={'/'} exact>
                    <SearchPage />
                </Route>
                <Route path={'*'}>
                    <Redirect to={'/'} />
                </Route>
            </Switch>
        </div>
    );
}
