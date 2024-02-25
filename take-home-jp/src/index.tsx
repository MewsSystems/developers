import React from 'react';
import ReactDOM from 'react-dom/client';
import './styling/globalStyles.css';
import AppRouting from './routes';

const root = ReactDOM.createRoot(
  document.getElementById('root') as HTMLElement,
);
root.render(
  <React.StrictMode>
    <AppRouting />
  </React.StrictMode>,
);
