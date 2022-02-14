import movieDetailReducer, {clearMovieDetail, MovieDetailState} from './movieDetailSlice';

describe('movie detail reducer', () => {
  const initialState: MovieDetailState = {
    movieDetail: null,
    status: 'idle',
    error: null,
  };
  it('should handle initial state', () => {
    expect(movieDetailReducer(undefined, {type: 'unknown'})).toEqual({
      movieDetail: null,
      status: 'idle',
      error: null,
    });
  });

  it('should handle clear movie detail', () => {
    const actual = movieDetailReducer(initialState, clearMovieDetail());
    expect(actual).toEqual({
      movieDetail: null,
      status: 'idle',
      error: null,
    });
  });
});
