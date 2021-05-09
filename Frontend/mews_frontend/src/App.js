import React from 'react';
import styled from 'styled-components';
import {
  BrowserRouter as Router, Route,
  Link

} from "react-router-dom";
import MovieDetail from './Components/MovieDetail';
import MovieList from './Components/MovieList';
import { Provider } from 'react-redux';
import store from './store';


const AppStyle = styled.div`
color:#fff;
text-align:center;
`
const Title = styled.h1`
color:#fff;

`;

function App() {

  return (
    <Provider store={store}>
      <Router>
        <AppStyle>
          <Link to='/'><Title>THE MOVIE DB</Title></Link>
          <Route exact path="/" component={MovieList} />
          <Route exact path="/movie/:id" component={MovieDetail} />
        </AppStyle>
      </Router>

    </Provider>



  );
}

export default App;
