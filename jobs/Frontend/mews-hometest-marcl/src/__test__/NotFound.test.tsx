import React from 'react';
import { render, screen, fireEvent } from '@testing-library/react';
import { MemoryRouter, useNavigate } from 'react-router-dom';
import NotFoundPage from '../components/movieComponent/NotFound';

describe('NotFoundPage Component', () => {
    test('renders the not found image', () => {
        render(
            <MemoryRouter>
              <NotFoundPage />
            </MemoryRouter>
          );
      const imageElement = screen.getByAltText('Popcorn');
      expect(imageElement).toBeInTheDocument();
    });
  
    test('renders the "Go Back" button', () => {
        render(
            <MemoryRouter>
              <NotFoundPage />
            </MemoryRouter>
          );
      const buttonElement = screen.getByText('Go Back');
      expect(buttonElement).toBeInTheDocument();
    });
});