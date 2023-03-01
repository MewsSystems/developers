import React from 'react';
import { Outlet } from 'react-router-dom';
import { Reset } from 'styled-reset'
import { Layout } from './Components/Layout/Layout';
import { createGlobalStyle } from 'styled-components'

const GlobalStyle = createGlobalStyle`
  html {
    font-size: 62.5%;
  }
`;

interface AppProps {
  outlet?: any;
}

const App = (props: AppProps) => {
  return (
    <Layout>
      <Reset />
      <GlobalStyle />
      {props.outlet ? props.outlet : <Outlet />}
    </Layout>
  );
}

export default App;
