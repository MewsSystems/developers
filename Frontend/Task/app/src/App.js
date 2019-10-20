import React, { useEffect, Suspense, useCallback } from 'react';
import './App.css';

import { useDispatch, useSelector } from 'react-redux'
import * as actions from './store/actions/index'

import { MainContent } from './pages/MainContent'

const App = props => {

  return (
      <>
          <MainContent></MainContent>
      </>
  );

}

export default App;
