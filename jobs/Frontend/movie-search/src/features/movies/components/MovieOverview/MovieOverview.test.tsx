import { Theme } from '@emotion/react';
import { SxProps } from '@mui/material';
import { render, screen } from '@testing-library/react';
import testData from '../../../mocks/test_movie.json';
import { Details } from '../../models/Details';
import MovieOverview from './MovieOverview';

const testMovie = testData as Details;
const sxPaperContainer: SxProps<Theme> = { p: 4, borderRadius: 2, maxWidth: '70rem', height: 'fit-content' };

it('renders the movie overview section', () => {
  render(<MovieOverview details={testMovie} sxPaperContainer={sxPaperContainer} />);

  const overview = screen.getByTestId('movie-overview-section');
  expect(overview).toBeInTheDocument();
});
