import React from 'react';
import styled from 'styled-components';
import Header from './Header';
import Footer from './Footer';
import Routing from '../../Routing';
import GlobalStyle from './GlobalStyle';

const Wrapper = styled.div`
  display: flex;
  flex-direction: column;
  height: 100%;
`;

const StyledFooter = styled(Footer)`
    text-align: center;
    padding: 10px;
    box-sizing: border-box;
    color: #333;
`;

const StyledHeader = styled(Header)``;

const StyledContent = styled(Routing)`
    flex: 1;
    margin: 0 auto;
    padding: 15px;
    max-width: 810px;
    width: 100%;
    box-sizing: border-box;
`;

export default function Layout() {
    return (
        <Wrapper>
            <GlobalStyle />
            <StyledHeader />
            <StyledContent />
            <StyledFooter />
        </Wrapper>
    );
}
