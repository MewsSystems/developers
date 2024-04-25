import { render, screen, within } from '@testing-library/react';
import { SearchDisplay } from './Search';
import { FC, PropsWithChildren, ReactElement, ReactNode } from 'react';
import userEvent from '@testing-library/user-event';
import { Provider } from 'react-redux';
import { setupStore } from '../../redux/store';
import { MovieState } from '../../redux/movies/movieSlice';
import { BrowserRouter } from 'react-router-dom';

const jackReacher = 'Jack Reacher';
const whenTheManComesAround = 'Jack Reacher: When the Man Comes Around';
const neverGoBack = 'Jack Reacher: Never Go Back';

const mockData = [
  {
    adult: false,
    backdrop_path: '/iwvP8XVpYVmJ3xfF9xdBi5uAOWl.jpg',
    genre_ids: [80, 18, 53, 28],
    id: 75780,
    original_language: 'en',
    original_title: jackReacher,
    overview:
      "One morning in an ordinary town, five people are shot dead in a seemingly random attack. All evidence points to a single suspect: an ex-military sniper who is quickly brought into custody. The interrogation yields one written note: 'Get Jack Reacher!'. Reacher, an enigmatic ex-Army investigator, believes the authorities have the right man but agrees to help the sniper's defense attorney. However, the more Reacher delves into the case, the less clear-cut it appears. So begins an extraordinary chase for the truth, pitting Jack Reacher against an unexpected enemy, with a skill for violence and a secret to keep.",
    popularity: 92.994,
    poster_path: '/uQBbjrLVsUibWxNDGA4Czzo8lwz.jpg',
    release_date: '2012-12-20',
    title: jackReacher,
    video: false,
    vote_average: 6.62,
    vote_count: 6748,
  },
  {
    adult: false,
    backdrop_path: null,
    genre_ids: [99],
    id: 1045592,
    original_language: 'en',
    original_title: whenTheManComesAround,
    overview:
      "Cast and crew speak on adapting One Shot as the first Jack Reacher film, casting Tom Cruise, earning Lee Child's blessing, additional character qualities and the performances that shape them, Lee Child's cameo in the film, and shooting the film's climax.",
    popularity: 11.277,
    poster_path: '/tcOPca5Ook6aR9mehrnxD9kfk7m.jpg',
    release_date: '2013-05-07',
    title: whenTheManComesAround,
    video: false,
    vote_average: 10,
    vote_count: 1,
  },
  {
    adult: false,
    backdrop_path: '/ww1eIoywghjoMzRLRIcbJLuKnJH.jpg',
    genre_ids: [28, 53],
    id: 343611,
    original_language: 'en',
    original_title: neverGoBack,
    overview:
      'When Major Susan Turner is arrested for treason, ex-investigator Jack Reacher undertakes the challenging task to prove her innocence and ends up exposing a shocking conspiracy.',
    popularity: 43.165,
    poster_path: '/cOg3UT2NYWHZxp41vpxAnVCOC4M.jpg',
    release_date: '2016-10-19',
    title: neverGoBack,
    video: false,
    vote_average: 5.974,
    vote_count: 4683,
  },
];

const mockSetSearchQuery = jest.fn().mockName('mockSetSearchQuery');
const mockIncrementPageNumber = jest.fn().mockName('mockIncrementPageNumber');
const mockDecrementPageNumber = jest.fn().mockName('mockDecrementPageNumber');
const mockDispatch = jest.fn().mockName('mockDispatch');

const mockBundleWithMovies = {
  movies: mockData,
  searchQuery: 'jack reacher',
  page: 1,
  numberOfPages: 1,
  setSearchQuery: mockSetSearchQuery,
  incrementPageNumber: mockIncrementPageNumber,
  decrementPageNumber: mockDecrementPageNumber,
  dispatch: mockDispatch,
};

const mockBundleWithNoMovies = {
  movies: [],
  searchQuery: '',
  page: 1,
  numberOfPages: 1,
  setSearchQuery: mockSetSearchQuery,
  incrementPageNumber: mockIncrementPageNumber,
  decrementPageNumber: mockDecrementPageNumber,
  dispatch: mockDispatch,
};

const setUp = (Component: ReactElement, mockState?: Partial<MovieState>) => {
  const mockStore = setupStore(mockState);

  return {
    user: userEvent.setup(),
    ...render(<Provider store={mockStore}>{Component}</Provider>, {
      wrapper: BrowserRouter,
    }),
  };
};

describe('SearchDisplay', () => {
  beforeAll(() => {
    jest.resetAllMocks();
  });

  it('renders a blank screen with no movie data', async () => {
    setUp(<SearchDisplay useMoviesBundle={mockBundleWithNoMovies} />);

    // Navigation controls should not be present when the search bar is empty.
    expect(
      screen.queryByRole('group', { name: 'navigation-controls-top' }),
    ).not.toBeInTheDocument();

    expect(
      screen.queryByRole('group', { name: 'navigation-controls-bottom' }),
    ).not.toBeInTheDocument();
  });

  it('renders with data', async () => {
    setUp(<SearchDisplay useMoviesBundle={mockBundleWithMovies} />);

    expect(await screen.findByText(jackReacher)).toBeInTheDocument();
    expect(await screen.findByText(neverGoBack)).toBeInTheDocument();
    expect(await screen.findByText(whenTheManComesAround)).toBeInTheDocument();

    expect(
      await screen.findByRole('group', { name: 'navigation-controls-top' }),
    ).toBeInTheDocument();
    expect(
      await screen.findByRole('group', { name: 'navigation-controls-bottom' }),
    ).toBeInTheDocument();

    expect(await screen.findAllByText('1 of 1')).toHaveLength(2);
  });

  it('calls the navigation buttons on click', async () => {
    const { user } = setUp(
      <SearchDisplay useMoviesBundle={mockBundleWithMovies} />,
    );

    const topNavigationControls = await screen.findByRole('group', {
      name: 'navigation-controls-top',
    });
    const topNextButton = await within(topNavigationControls).findByText(
      /next/i,
    );
    const topBackButton = await within(topNavigationControls).findByText(
      /back/i,
    );

    await user.click(topNextButton);

    expect(mockIncrementPageNumber).toHaveBeenCalledTimes(1);

    await user.click(topBackButton);

    expect(mockDecrementPageNumber).toHaveBeenCalledTimes(1);

    const bottomNavigationControls = await screen.findByRole('group', {
      name: 'navigation-controls-bottom',
    });
    const bottomNextButton = await within(bottomNavigationControls).findByText(
      /next/i,
    );
    const bottomBackButton = await within(bottomNavigationControls).findByText(
      /back/i,
    );

    await user.click(bottomNextButton);

    expect(mockIncrementPageNumber).toHaveBeenCalledTimes(2);

    await user.click(bottomBackButton);

    expect(mockDecrementPageNumber).toHaveBeenCalledTimes(2);
  });
});
