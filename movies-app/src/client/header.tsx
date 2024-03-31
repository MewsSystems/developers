import React from 'react';
import { useNavigate } from "react-router-dom";
import { ReactComponent as Logo } from './common/images/logo.svg';
import { HeaderWrapper, LogoWrapper } from './header.styled';

export const Header = () => {
    const navigate = useNavigate();
    const goToHomePage = () => navigate(`/`);

    return (
        <HeaderWrapper onClick={goToHomePage}>
            <LogoWrapper>
                <Logo/>
            </LogoWrapper>
            <h1>
                MovieMantra
            </h1>
        </HeaderWrapper>
    );
};