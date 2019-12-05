import React, {useState, useEffect} from 'react';
import {useDispatch} from 'react-redux';
import apiCall from './api/apiCall';
import PairSelector from './components/PairSelector';
import RatesTable from './components/RatesTable';
import {setCurrencyPairs} from './actions/pairsActions';
import styled, {createGlobalStyle} from 'styled-components';
import {Loader} from './ui';
import Countly from 'countly-sdk-web';
import countlyConfig from './countly'

const App = () => {
  const {loading} = useConfigurationFetcher ();

  const {url, app_key} = countlyConfig;
  useAnalytics(url, app_key)
  
  if (loading) return <Loader />;
  return (
    <AppWrapper>
      <PairSelector />
      <RatesTable />
      <GlobalStyle />
    </AppWrapper>
  );
};

const AppWrapper = styled ('div')`
  margin: 0 auto;
  max-width: 90%;
  margin-bottom: 100px;
`;

const GlobalStyle = createGlobalStyle`
  @import url("https://fonts.googleapis.com/css?family=Source+Sans+Pro:400,700");
  body {
    padding: 24px;
    font-family: "Source Sans Pro", sans-serif;
    margin: 0;
    background: white;
  }
`;

export default App;

const useConfigurationFetcher = () => {
  const dispatch = useDispatch ();
  const [loading, setLoading] = useState (true);
  const setPairs = pairs => {
    dispatch (setCurrencyPairs (pairs));
    setLoading (false);
  };
  useEffect (() => {
    const processConfig = data => setPairs (data.currencyPairs);
    apiCall ('/configuration').then (processConfig);
  });
  return {loading};
};

const useAnalytics = (url, app_key) => {
  const countlyParams = {
    app_key,
    url,
  };
  Countly.init (countlyParams);
  Countly.track_sessions ();
}
