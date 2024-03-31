import React from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import { Header } from './header';
import { MainPage } from './main/main';
import { MoviePage } from './movie/movie-page';
import { GlobalStyle, AppWrapper } from './app.styled';
import { GlobalFonts } from './common/fonts/typography.styled';
import { NotFound } from './not-found/not-found';
import './common/styles/reset.css';

export const App = () => {
    return (
        <>
            <GlobalFonts/>
            <GlobalStyle/>
            <AppWrapper>
                <Router>
                    <Header/>
                    <Routes>
                        <Route path="/" Component={MainPage}/>
                        <Route path="/movie/:id" Component={MoviePage}/>
                        <Route path="*" element={<NotFound/>}/>
                    </Routes>
                </Router>
            </AppWrapper>
        </>
    );
};