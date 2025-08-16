import React from 'react';
import { Outlet } from 'react-router-dom';

import { NavBar } from './components/NavBar';
import { Body } from './styles/styles';
import SearchComponent from './components/SearchComponent';
import ApiConnector from './ApiConnector';

export const App = () => {
  return (
    <>
      <ApiConnector />
      <NavBar />
      <Body>
        <SearchComponent />
        <Outlet />
      </Body>
    </>
  );
};
