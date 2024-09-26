import React from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import { Box } from '@mui/material';
import MainView from "./MainView";
import MovieDetailsWrapper from './MovieDetailsWrapper';
import NotFoundPage from './movieComponent/NotFound';
import '../styles/App.css';

const App: React.FC = () => {
  return (
    <Router>
      <Box className="background">
        <div data-testid="background" />
      </Box>
      <Routes>
        <Route path="/" element={<MainView />} />
        <Route path="/details/:id" element={<MovieDetailsWrapper />} />
        <Route path="*" element={<NotFoundPage />} />
      </Routes>
    </Router>
  );
};

export default App;
