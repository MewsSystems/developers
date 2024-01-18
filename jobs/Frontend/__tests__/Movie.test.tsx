import { renderWithProviders } from '@/utils/test-utils';
import { act, fireEvent, screen, waitFor } from '@testing-library/react';
import SearchLayout from '@/Layouts/Search';
import { http, HttpResponse, delay } from 'msw';
import { setupServer } from 'msw/node';
import { setSearchMovieAction } from '@/store';
import { singleMovieMock } from '../mocks/movie';
import Movie from '@/pages/movie/[id]';
import MovieLayout from '@/Layouts/Movie';

export const handlers = [
  http.get(`https://api.themoviedb.org/3/movie/:id`, async (...args) => {
    return HttpResponse.json({...singleMovieMock})
  })
]

const server = setupServer(...handlers)
beforeAll(() => server.listen())
afterEach(() => server.resetHandlers())
afterAll(() => server.close())


test('Renders movie detail', async () => {
  const t = renderWithProviders(<MovieLayout id={11}/>);
  await waitFor(async () => expect(screen.findByText('Star Wars')));
});
