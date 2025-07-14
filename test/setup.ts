process.env.MOVIE_DB_ACCESS_TOKEN = 'test-token';

import '@testing-library/jest-dom';
import { server } from './server';

beforeAll(() => server.listen());
afterEach(() => server.resetHandlers());
afterAll(() => server.close());
