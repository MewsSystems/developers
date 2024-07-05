import { screen, within } from '@testing-library/react';
import { SearchDisplay } from './Search';
import {
  jackReacher,
  mockData,
  neverGoBack,
  whenTheManComesAround,
} from '../../mockData/mockData';
import { setUp } from '../../utils/testUtils';

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
