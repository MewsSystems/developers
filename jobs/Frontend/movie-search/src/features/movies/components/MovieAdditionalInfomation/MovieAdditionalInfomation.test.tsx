import { Theme } from '@emotion/react';
import { SxProps } from '@mui/material';
import { render, screen } from '@testing-library/react';
import { Details } from '../../models/Details';
import MovieAdditionalInformation from './MovieAdditionalInfomation';

const sxPaperContainer: SxProps<Theme> = { p: 4, borderRadius: 2, maxWidth: '70rem', height: 'fit-content' };

it('renders the additional movie information section', async () => {
  render(<MovieAdditionalInformation details={{} as Details} sxPaperContainer={sxPaperContainer} />);

  const additionalInformation = screen.getByTestId('movie-additional-information-section');
  expect(additionalInformation).toBeInTheDocument();
});
