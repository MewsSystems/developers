import React from 'react';
import { ReactComponent as Logo } from './common/images/logo.svg';
import './header.scss';

export const Header = () => {
    return (
        <header className="header">
            <div className="header__logo">
                <Logo />
            </div>
            <h1 className="header__title">
                MovieMantra
            </h1>
        </header>
    );
};