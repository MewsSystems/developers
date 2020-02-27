import { processRequest } from '../../services/Api';
import * as sagas from '../MoviesSagas';
import { put, call } from 'redux-saga/effects';
import * as moviesActions from '../MoviesActions';

describe('Get movies saga tests', () => {
  const action = {
    payload: {
        query: "Harry",
        page: 1,
    },
  };
  const url = `search/movie?query=${query}&page=${page}&`

  it('Should successfully get movies', () => {
    const generator = sagas.handleGetMoviesRequest(action);

    let next = generator.next();

    expect(next.value).toEqual(call(processRequest, url));

    next = generator.next((call(processRequest, url)));
    expect(next.value).toEqual(put(moviesActions.fetchMoviesSuccess(call(processRequest, url).data)));

    next = generator.next();
    expect(next.done).toEqual(true);
  });

  it('Should failed when try to get movies', () => {
    const generator = sagas.handleGetMoviesRequest(action);

    let next = generator.next();

    expect(next.value).toEqual(call(processRequest, url));

    next = generator.throw(new Error('500 Internal Server Error'));
    expect(next.value).toEqual(put(moviesActions.fetchMoviesError(new Error('500 Internal Server Error'))));

    next = generator.next();
    expect(next.done).toEqual(true);
  });
});