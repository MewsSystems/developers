import { fork } from 'redux-saga/effects';
import moviesSagas from './movies/MoviesSagas';

export default function* rootSaga() {
  yield fork(moviesSagas);
}
