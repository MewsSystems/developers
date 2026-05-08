/* eslint-disable @typescript-eslint/no-explicit-any */
import { createMemoryRouter, RouterProvider } from 'react-router';
import { vi } from 'vitest';
import { ThemeProvider } from 'styled-components';
import { render, screen, waitFor } from '@testing-library/react';
import { DetailsMoviePage } from '../DetailsMoviePage';
import { useGetDetailsMovie } from '../../../hooks';
import { QueryClientWrapper } from '../../../utils/testUtils/QueryClientWrapper';
import { lightTheme } from '../../../styles/themes';
import { detailsMovieAdapter } from '../../../adapters/detailsMovieAdapter';
import {
  CARD_DETAILS_MOVIE_SKELETON_TEST_ID,
  GO_BACK_BUTTON_TEST_ID,
  MOCKED_DETAILS_MOVIE,
} from '../../../constants';
import { MOCK_DETAILS_MOVIE } from '../constants';
import { Header } from '../../../components';
import userEvent from '@testing-library/user-event';
import { ListMoviePage } from '../../ListMoviesPage/ListMoviePage';

vi.mock('../../../hooks/useGetDetailsMovie');
vi.mock('../../../api/fetch/fetchDetailsMovie');
vi.mock('../../../adapters/detailsMovieAdapter', () => ({
  detailsMovieAdapter: vi.fn(),
}));

const routes = [
  {
    path: '/',
    element: (
      <ThemeProvider theme={lightTheme}>
        <QueryClientWrapper>
          <ListMoviePage />
        </QueryClientWrapper>
      </ThemeProvider>
    ),
  },

  {
    path: '/details/1',
    element: (
      <ThemeProvider theme={lightTheme}>
        <QueryClientWrapper>
          <Header />
          <DetailsMoviePage />
        </QueryClientWrapper>
      </ThemeProvider>
    ),
  },
];

const router = createMemoryRouter(routes, {
  initialEntries: ['/details/1'],
});

const renderDetailsMovie = () => {
  render(<RouterProvider router={router} />);
};
const mockUseGetDetailsMovie = ({ isLoading }: { isLoading: boolean }) => {
  return (useGetDetailsMovie as any).mockReturnValue({
    data: MOCKED_DETAILS_MOVIE,
    isLoading,
  });
};

describe('DetailsMoviePage', () => {
  beforeEach(() => {
    mockUseGetDetailsMovie({ isLoading: false });
    (detailsMovieAdapter as any).mockReturnValue(MOCK_DETAILS_MOVIE);
  });

  it('renders card details movie skeleton while loading', () => {
    mockUseGetDetailsMovie({ isLoading: true });
    renderDetailsMovie();

    expect(screen.getByTestId(CARD_DETAILS_MOVIE_SKELETON_TEST_ID)).toBeInTheDocument();
  });

  it('renders details movie when data is available', async () => {
    mockUseGetDetailsMovie({ isLoading: false });
    renderDetailsMovie();

    expect(await screen.findByText('Ariel')).toBeInTheDocument();
  });

  it('navigate to list movies page when the "Return to search" button is clicked', async () => {
    mockUseGetDetailsMovie({ isLoading: false });
    renderDetailsMovie();

    const goBackButton = screen.getByTestId(GO_BACK_BUTTON_TEST_ID);

    expect(goBackButton).toBeInTheDocument();

    await waitFor(() => {
      userEvent.click(goBackButton);
      expect(router.state.location.pathname).toBe('/');
    });
  });
});
