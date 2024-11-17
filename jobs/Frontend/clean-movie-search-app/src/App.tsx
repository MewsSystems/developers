import React from 'react';
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { SearchView } from './views/SearchView';
import { MovieDetailsView } from './views/MovieDetailsView';
import { MovieProvider } from './context/MovieContext';
import styled from 'styled-components';

const Container = styled.div`
  max-width: 1200px;
  margin: 0 auto;
  padding: 20px;
`;

const Title = styled.h1`
  font-size: 2rem;
  color: #333;
  margin-bottom: 24px;
`;

function App() {
  return (
    <MovieProvider>
      <BrowserRouter>
        <Container>
          <Title>What to watch</Title>
          <Routes>
            <Route path="/search" element={<SearchView />} />
            <Route path="/movie/:id" element={<MovieDetailsView />} />
            <Route path="/" element={<Navigate to="/search" replace />} />
          </Routes>
        </Container>
      </BrowserRouter>
    </MovieProvider>
  );
}

export default App;
