import { renderWithProviders } from '@/utils/test-utils';
import { act, fireEvent, screen, waitFor } from '@testing-library/react';
import SearchLayout from '@/Layouts/Search';
import { http, HttpResponse, delay } from 'msw';
import { setupServer } from 'msw/node';
import { movie1, movie2, movie3 } from '../mocks/movies';
import { setSearchMovieAction } from '@/store';

export const handlers = [
  http.get(`https://api.themoviedb.org/3/search/movie`, async (...args) => {
    return HttpResponse.json({
      page: 1,
      total_pages: 1,
      total_results: 3,
      results: [
        movie1,
        movie2,
        movie3
      ]
    })
  })
]

const server = setupServer(...handlers)
beforeAll(() => server.listen())
afterEach(() => server.resetHandlers())
afterAll(() => server.close())


test('Renders Search and Default pagination', () => {
  renderWithProviders(<SearchLayout/>);

  expect(screen.getByText(/Search Movie:/i));
  expect(screen.queryByTestId('page-3-button')).toBe(null);
});

test('Searches for a movie', async () => {
  const t = renderWithProviders(<SearchLayout/>);
  await act(() => t.store?.dispatch(setSearchMovieAction('Star Wars')));

  expect(screen.getByText('loading...'));
  expect(await screen.findByText(/movie:/i));
});
