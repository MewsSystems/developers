import React from 'react';
import renderer from 'react-test-renderer';
import { render, screen, fireEvent } from '@testing-library/react';
import '@testing-library/jest-dom';
import Home from './';

it('renders correctly', () => {
  const tree = renderer.create(<Home />).toJSON();
  expect(tree).toMatchSnapshot();
});

it('should show results after typing into search bar', () => {
  render(<Home />);
  const inputElement = screen.getByLabelText(/Search for a movie/i);
  fireEvent.change(inputElement, { target: { value: 'Godfather' } });
});
