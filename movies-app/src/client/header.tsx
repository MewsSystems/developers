import React from 'react';
import { useNavigate } from "react-router-dom";
import { ReactComponent as Logo } from './common/images/logo.svg';
import './header.scss';

export const Header = () => {
    const navigate = useNavigate();
    const goToHomePage = () => navigate(`/`);

    return (
        <header className="header" onClick={goToHomePage}>
            <div className="header__logo">
                <Logo />
            </div>
            <h1 className="header__title">
                MovieMantra
            </h1>
        </header>
    );
};