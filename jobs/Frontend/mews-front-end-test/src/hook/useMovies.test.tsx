import { act, renderHook } from '@testing-library/react';
import { useMovies } from './useMovies';
import { RootState, setupStore } from '../redux/store';
import { Provider } from 'react-redux';
import { ReactElement, ReactNode } from 'react';

const createWrapper = (
  mockedState, // mockedState: Partial<RootState>,
): ((prop: { children: ReactNode }) => ReactElement) => {
  const store = setupStore();

  return ({ children }) => <Provider store={store}>{children}</Provider>;
};

const setUp = () => {
  const result = renderHook(() => useMovies(), { wrapper: createWrapper() });

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

  it('does function stuff', () => {
    const { result } = setUp();

    expect(result.current.page).toBe(1);

    act(() => {
      result.current.incrementPageNumber();
    });

    expect(result.current.page).toBe(2);
  });
});
