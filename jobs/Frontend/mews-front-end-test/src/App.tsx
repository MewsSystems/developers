import React from 'react';
import './App.css';
import { StyledAppContainer } from './App.styled';
import { Search } from './components/Search/Search';
import { Header } from './components/Header/Header';

function App() {
  return (
    <StyledAppContainer>
      <Header />
      <Search />
    </StyledAppContainer>
  );
}

export default App;
