import { processRequest } from '../../services/Api';
import * as sagas from '../MovieDetailsSagas';
import { put, call } from 'redux-saga/effects';
import * as movieDetailsActions from '../MovieDetailsActions';

describe('Get movie details saga tests', () => {
  const action = {
    payload: {
      id: 671,
    },
  };
  const url = `movie/${action.payload.id}?`

  it('Should successfully get movie details', () => {
    const generator = sagas.handleGetMovieDetailsRequest(action);

    let next = generator.next();

    expect(next.value).toEqual(call(processRequest, url));

    next = generator.next((call(processRequest, url)));
    expect(next.value).toEqual(put(movieDetailsActions.fetchMovieDetailsSuccess(call(processRequest, url).data)));

    next = generator.next();
    expect(next.done).toEqual(true);
  });

  it('Should failed when try to get movie details', () => {
    const generator = sagas.handleGetMovieDetailsRequest(action);

    let next = generator.next();

    expect(next.value).toEqual(call(processRequest, url));

    next = generator.throw(new Error('500 Internal Server Error'));
    expect(next.value).toEqual(put(movieDetailsActions.fetchMovieDetailsError(new Error('500 Internal Server Error'))));

    next = generator.next();
    expect(next.done).toEqual(true);
  });
});