import { put, call, takeLatest, all } from 'redux-saga/effects';
import { processRequest } from '../services/Api';
import { moviesActionTypes } from './MoviesConstants';
import * as moviesActions from './MoviesActions';

export default function* () {
  yield all([
    yield takeLatest(moviesActionTypes.FETCH_MOVIES, handleGetMoviesRequest),
  ]);
}

export function* handleGetMoviesRequest(action) {
  try {
    const { query, page } = action.payload;
    const {data} = yield call(processRequest, `search/movie?query=${query}&page=${page}&`);

    yield put(moviesActions.fetchMoviesSuccess(data));
  } catch(e) {
    yield put(moviesActions.fetchMoviesError(e));
  }
}
