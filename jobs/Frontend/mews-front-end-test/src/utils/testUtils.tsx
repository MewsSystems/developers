import { ReactElement } from 'react';
import { MovieState } from '../redux/movies/movieSlice';
import { setupStore } from '../redux/store';
import userEvent from '@testing-library/user-event';
import { render } from '@testing-library/react';
import { Provider } from 'react-redux';
import { BrowserRouter } from 'react-router-dom';

const setUp = (Component: ReactElement, mockState?: Partial<MovieState>) => {
  const mockStore = setupStore(mockState);

  return {
    user: userEvent.setup(),
    ...render(<Provider store={mockStore}>{Component}</Provider>, {
      wrapper: BrowserRouter,
    }),
  };
};

export { setUp };
