import React from 'react';
import './App.css';

import MainContent from './pages/MainContent'
import Header from './components/Header/Header'

const App = props => {

  return (
      <>
          <Header></Header>
          <MainContent></MainContent>
      </>
  );

}

export default App;
