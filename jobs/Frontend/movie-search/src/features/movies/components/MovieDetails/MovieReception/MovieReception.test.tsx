import { Theme } from '@emotion/react';
import { SxProps } from '@mui/material';
import { render, screen } from '@testing-library/react';
import { expect, it } from 'vitest';
import testData from '../../../../mocks/test_movie.json';
import { Details } from '../../../models/Details';
import MovieReception from './MovieReception';

const testMovie = testData as Details;
const sxPaperContainer: SxProps<Theme> = { p: 4, borderRadius: 2, maxWidth: '70rem', height: 'fit-content' };

it('renders the movie reception section', () => {
  render(<MovieReception details={testMovie} sxPaperContainer={sxPaperContainer} />);

  const reception = screen.getByTestId('movie-reception-section');
  expect(reception).toBeInTheDocument();
});
