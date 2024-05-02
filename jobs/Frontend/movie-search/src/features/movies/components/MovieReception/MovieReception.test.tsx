import { Theme } from '@emotion/react';
import { SxProps } from '@mui/material';
import { render, screen } from '@testing-library/react';
import { Details } from '../../models/Details';
import MovieReception from './MovieReception';

const sxPaperContainer: SxProps<Theme> = { p: 4, borderRadius: 2, maxWidth: '70rem', height: 'fit-content' };

it('renders the movie reception section', async () => {
  render(<MovieReception details={{} as Details} sxPaperContainer={sxPaperContainer} />);

  const reception = screen.getByTestId('movie-reception-section');
  expect(reception).toBeInTheDocument();
});
