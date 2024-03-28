import React from 'react';
import {BrowserRouter as Router, Route, Routes} from 'react-router-dom';
import { useInjection } from 'inversify-react';
import {MoviesApi} from '../data/api/movies-api.store';
import {Header} from './header';
import {MainPage} from './main/main';
import {MoviePage} from './movie/movie';
import './app.scss';

export const App = () => {
    const moviesApi = useInjection(MoviesApi);

    return (
        <div className="app">
            <Router>
                <Header/>
                <Routes>
                    <Route path="/" Component={MainPage}/>
                    <Route path="/movie" Component={MoviePage}/>
                </Routes>
            </Router>
        </div>
    );
};