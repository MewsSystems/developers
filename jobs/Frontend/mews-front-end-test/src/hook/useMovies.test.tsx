import { act, renderHook } from '@testing-library/react';
import { useMovies } from './useMovies';
import { RootState, setupStore } from '../redux/store';
import { Provider } from 'react-redux';
import { ReactElement, ReactNode } from 'react';
import * as selectors from '../redux/selectors';

jest.mock('../redux/selectors');

const createWrapper = (
  mockedState?: Partial<RootState>,
): ((prop: { children: ReactNode }) => ReactElement) => {
  const store = setupStore(mockedState);

  return ({ children }) => <Provider store={store}>{children}</Provider>;
};

const setUp = (mockedState?: Partial<RootState>) => {
  const result = renderHook(() => useMovies(), {
    wrapper: createWrapper(mockedState),
  });

  return { result: result.result };
};

describe('useMovies', () => {
  it('works', () => {
    const {
      result: { current },
    } = setUp();

    expect(Array.isArray(current.movies)).toEqual(Array.isArray([]));
    expect(current.searchQuery).toBe('');
    expect(current.page).toBe(1);

    expect(current.numberOfPages).toBe(1);
  });

  it('correctly increments and decrements the page number when called incrementPageNumber and decrementPageNumber', () => {
    jest
      .spyOn(selectors, 'getSearchQuerySelector')
      .mockImplementation(() => 'ab');
    jest.spyOn(selectors, 'getPageSelector').mockImplementation(() => 1);
    jest
      .spyOn(selectors, 'getNumberOfPagesSelector')
      .mockImplementation(() => 10);

    const { result } = setUp();

    expect(result.current.page).toBe(1);

    act(() => {
      result.current.incrementPageNumber();
    });

    expect(result.current.page).toBe(2);

    act(() => {
      result.current.decrementPageNumber();
    });

    expect(result.current.page).toBe(1);
  });
});
