
import { movieService } from "../movieService";
import { FetchProps } from "../testUtils/FetchProps";

describe('moveService', () => {
  let globalFetch: FetchProps;
  const mockJson = jest.fn();

  beforeEach(() => {
    globalFetch = global.fetch;
    global.fetch = jest.fn().mockImplementation(() => ({
      json: mockJson,
    }));
  });

  afterEach(() => {
    global.fetch = globalFetch;
    mockJson.mockClear();
  });

  it('should be able to search for movies', async () => {
    mockJson.mockResolvedValue({
      page: 0,
      results: [],
    });

    const actual = await movieService.search('dune', 1);
    expect(actual).toEqual({
      page: 0,
      movies: []
    });
  });
});
