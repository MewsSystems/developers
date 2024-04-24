import React, { useEffect } from 'react';
import './App.css';
import { StyledAppContainer } from './App.styled';
import { Search } from './components/Search/Search';
import { Header } from './components/Header/Header';
import { MovieApiResponse, sendRequest } from './api/sendRequest';

function App() {
  useEffect(() => {
    sendRequest()
      .then((response: MovieApiResponse) => {
        console.log('response: ', response);
      })
      .catch((error) => {
        console.error(error);
      });
  }, []);

  return (
    <StyledAppContainer>
      <Header />
      <Search />
    </StyledAppContainer>
  );
}

export default App;
