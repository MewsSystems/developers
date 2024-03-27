import React from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import { Header } from './header';
import { MainPage } from './main/main';
import { MoviePage } from './movie/movie';
import './common/styles/reset.css';

export const App = () => {
    return (
        <div className="app">
            <Router>
                <Header />
                <Routes>
                    <Route path="/" Component={MainPage} />
                    <Route path="/movie" Component={MoviePage} />
                </Routes>
            </Router>
        </div>
    );
};