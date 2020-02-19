import { fork } from 'redux-saga/effects';
import moviesSagas from './movies/MoviesSagas';
import movieDetailsSagas from './movieDetails/MovieDetailsSagas';

export default function* rootSaga() {
  yield fork(moviesSagas);
  yield fork(movieDetailsSagas);
}
