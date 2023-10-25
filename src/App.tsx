import React from 'react';
import { Outlet } from 'react-router-dom';

import { NavBar } from './components/NavBar';
import { Body } from './styles/styles';
import { SearchBar } from './components/SearchBar';

export const App = () => {
  return (
    <>
      <NavBar />
      <Body>
        <SearchBar />
        <Outlet />
      </Body>
    </>
  );
};
