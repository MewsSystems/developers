import { Theme } from '@emotion/react';
import { SxProps } from '@mui/material';
import { render, screen } from '@testing-library/react';
import testData from '../../../mocks/test_movie.json';
import { Details } from '../../models/Details';
import MovieAdditionalInformation from './MovieAdditionalInfomation';

const testMovie = testData as Details;
const sxPaperContainer: SxProps<Theme> = { p: 4, borderRadius: 2, maxWidth: '70rem', height: 'fit-content' };

it('renders the additional movie information section', () => {
  render(<MovieAdditionalInformation details={testMovie} sxPaperContainer={sxPaperContainer} />);

  const additionalInformation = screen.getByTestId('movie-additional-information-section');
  expect(additionalInformation).toBeInTheDocument();
});
