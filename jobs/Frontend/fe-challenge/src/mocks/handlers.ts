import { movieDetailMock, moviesResultMock } from '@/mocks/data';
import { http, HttpResponse, type RequestHandler } from 'msw';

export const handlers: RequestHandler[] = [
  http.get('https://api.themoviedb.org/3/search/movie', ({ request }) => {
    const requestUrl = new URL(request.url);
    const page = Number(requestUrl.searchParams.get('page'));
    const currentPage = page - 1;
    return HttpResponse.json(moviesResultMock[currentPage % 2]);
  }),

  http.get('https://api.themoviedb.org/3/movie/:id', () => {
    return HttpResponse.json(movieDetailMock);
  }),

  http.get('https://mock-api', () => {
    return HttpResponse.json({ result: 'mock-result' });
  }),
];
