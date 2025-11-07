import { HttpResponse, http as mswHttp } from 'msw';
import { FetchHttpRepository } from '@/modules/movies/infrastructure/FetchHttpRepository';
import { server } from '@/mocks/server';

describe('FetchHttpRepository', () => {
  beforeAll(() => server.listen());
  afterEach(() => server.resetHandlers());
  afterAll(() => server.close());

  test('should return response data when doing a get request', async () => {
    const http = new FetchHttpRepository();
    const response = await http.get('https://mock-api');

    expect(response).toEqual({ result: 'mock-result' });
  });

  test('should throw an error when get request is not ok', async () => {
    const http = new FetchHttpRepository();

    server.use(
      mswHttp.get(
        'https://mock-api',
        () => {
          return HttpResponse.json({ message: 'Error' }, { status: 400 });
        },
        { once: true },
      ),
    );

    expect(http.get('https://mock-api')).rejects.toThrow(
      new Error('Failed to fetch data'),
    );
  });
});
