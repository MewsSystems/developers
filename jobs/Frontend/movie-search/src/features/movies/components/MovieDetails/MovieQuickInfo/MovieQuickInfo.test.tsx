import { render, screen } from '@testing-library/react';
import testData from '../../../../mocks/test_movie.json';
import { Details } from '../../../models/Details';
import MovieQuickInfo from './MovieQuickInfo';

const testMovie = testData as Details;

it('renders the movie quick info section', () => {
  render(<MovieQuickInfo details={testMovie} title="Movie Title" />);

  const quickInfo = screen.getByTestId('movie-quick-info-section');
  expect(quickInfo).toBeInTheDocument();
});
