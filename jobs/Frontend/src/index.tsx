import React from 'react';
import { createRoot } from 'react-dom/client';
import Router from './router';
import CssBaseline from '@mui/material/CssBaseline';
import GlobalStyles from './styles/global';

const container = document.getElementById('movie-search-app');
const root = createRoot(container as HTMLElement);
root.render(
  <>
    <CssBaseline />
    <GlobalStyles />
    <Router />
  </>,
);
