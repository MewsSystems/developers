import { GET } from '@/app/api/movies/route';
import { http, HttpResponse } from 'msw';
import { server } from '@/test/server';
import { NextRequest } from 'next/server';

const mockSearchResponse = {
  page: 1,
  results: [
    {
      id: 1,
      title: 'Matrix',
      poster_path: '/matrix.jpg',
    },
  ],
};

const mockConfigResponse = {
  images: {
    secure_base_url: 'https://image.tmdb.org/t/p/',
    poster_sizes: ['w154'],
  },
};

describe('GET /api/movies?search=...', () => {
  it('returns 200 and enriched movie search results with poster_url', async () => {
    server.use(
      http.get('https://api.themoviedb.org/3/search/movie', () =>
        HttpResponse.json(mockSearchResponse)
      ),
      http.get('https://api.themoviedb.org/3/configuration', () =>
        HttpResponse.json(mockConfigResponse)
      )
    );

    const url = new URL('http://localhost/api/movies?search=matrix');
    const req = new NextRequest(url);
    const res = await GET(req);
    const json = await res.json();

    expect(res.status).toBe(200);
    expect(json.results[0]).toHaveProperty('poster_url');
    expect(json.results[0].poster_url).toHaveProperty('default');
    expect(json.results[0].poster_url.default).toBe('https://image.tmdb.org/t/p/w154/matrix.jpg');
  });

  it("returns 400 if 'search' param is missing", async () => {
    const url = new URL('http://localhost/api/movies');
    const req = new NextRequest(url);
    const res = await GET(req);
    const json = await res.json();

    expect(res.status).toBe(400);
    expect(json).toEqual({ error: 'Invalid query' });
  });

  it("returns 400 if 'page' param is not a positive integer", async () => {
    const url = new URL('http://localhost/api/movies?search=matrix&page=zero');
    const req = new NextRequest(url);
    const res = await GET(req);
    const json = await res.json();

    expect(res.status).toBe(400);
    expect(json).toEqual({ error: 'Invalid query' });
  });

  it('returns 500 if movie search fetch fails', async () => {
    server.use(
      http.get('https://api.themoviedb.org/3/search/movie', () =>
        HttpResponse.json({ error: 'fail' }, { status: 500 })
      ),
      http.get('https://api.themoviedb.org/3/configuration', () =>
        HttpResponse.json(mockConfigResponse)
      )
    );

    const url = new URL('http://localhost/api/movies?search=matrix');
    const req = new NextRequest(url);
    const res = await GET(req);
    const json = await res.json();

    expect(res.status).toBe(500);
    expect(json).toEqual({ error: 'unable to get movies' });
  });

  it('returns 500 if config fetch fails', async () => {
    server.use(
      http.get('https://api.themoviedb.org/3/search/movie', () =>
        HttpResponse.json(mockSearchResponse)
      ),
      http.get('https://api.themoviedb.org/3/configuration', () =>
        HttpResponse.json({ error: 'fail' }, { status: 500 })
      )
    );

    const url = new URL('http://localhost/api/movies?search=matrix');
    const req = new NextRequest(url);
    const res = await GET(req);
    const json = await res.json();

    expect(res.status).toBe(500);
    expect(json).toEqual({ error: 'unable to get movies' });
  });
});
