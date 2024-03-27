import React from 'react';
import {BrowserRouter as Router, Route, Routes} from 'react-router-dom';
import {MoviesApi} from '../data/api/movies-api.store';
import {Header} from './header';
import {MainPage} from './main/main';
import {MoviePage} from './movie/movie';
import './common/styles/reset.css';

const moviesApi = new MoviesApi();

export const App = () => {
    moviesApi.init().then(
        response => {
            console.log('response', response);
        },
        error => {
            console.error('error', error);
        }
    );

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