import { http, HttpResponse } from 'msw';

export const handlers = [
  http.get('https://api.example.com/user', () => {
    return HttpResponse.json({ id: 1, name: 'Alice' });
  }),
];
