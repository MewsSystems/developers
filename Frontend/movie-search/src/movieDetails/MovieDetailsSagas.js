import { put, call, takeLatest, all } from 'redux-saga/effects';
import { processRequest } from '../services/Api';
import { movieDetailsActionTypes } from './MovieDetailsConstants';
import * as movieDetailsActions from './MovieDetailsActions';

export default function* () {
  yield all([
    yield takeLatest(movieDetailsActionTypes.FETCH_MOVIE_DETAILS, handleGetMovieDetailsRequest),
  ]);
}

export function* handleGetMovieDetailsRequest(action) {
  try {
    const { id } = action.payload;
    const {data} = yield call(processRequest, `movie/${id}?`);

    yield put(movieDetailsActions.fetchMovieDetailsSuccess(data));
  } catch(e) {
    yield put(movieDetailsActions.fetchMovieDetailsError(e));
  }
}
