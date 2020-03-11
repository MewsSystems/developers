import React from 'react';
import { BrowserRouter as Router, Switch, Route } from 'react-router-dom';
import { useSelector } from 'react-redux';
import { ThemeProvider } from 'styled-components';

import MovieDetail from './MovieDetail';
import Home from './Home';
import Modal from './Modal';

import { Container } from './styled';

const lightTheme = {
  background: { primary: '#d3d3d3', withContent: 'white', hover: '#eeeeee' },
  border: { primary: 'gray' },
  text: { primary: 'black', disabled: 'gray' }
};
const darkTheme = {
  background: { primary: '#202020', withContent: 'black', hover: '#404040' },
  border: { primary: 'gray' },
  text: { primary: 'white', disabled: 'gray' }
};

const App = () => {
  const detailIsLoading = useSelector(state => state.detail.isLoading);
  const moviesIsLoading = useSelector(state => state.movies.isLoading);
  return (
    <ThemeProvider theme={lightTheme}>
      <Container>
        <Router>
          <Switch>
            <Route path='/:id'>
              <MovieDetail />
            </Route>
            <Route path='/'>
              <Home />
            </Route>
          </Switch>
        </Router>
      </Container>
      <Modal isOpen={detailIsLoading || moviesIsLoading | false}>Loading...</Modal>
    </ThemeProvider>
  );
};

export default App;
