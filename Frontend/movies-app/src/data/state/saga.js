import { put, takeEvery, select } from 'redux-saga/effects';
import * as actions from './actions';
import actionTypes from './actionTypes';

import * as api from './../../data/movieApi';

function* fetchMovies(action) {
  try {
    const result = yield api.getAll(action.searchTerm, action.page);
    yield put(actions.MoviesLoaded(result.data));
  } catch (error) {
    yield put(actions.MoviesError(error));
  }
}

function* fetchMoviesPage(action) {
  try {
    const searchTerm = yield select(state => state.movies.searchTerm);
    const result = yield api.getAll(searchTerm, action.page);
    yield put(actions.MoviesLoaded(result.data));
  } catch (error) {
    yield put(actions.MoviesError(error));
  }
}

function* fetchDetail(action) {
  try {
    const result = yield api.getDetail(action.id);
    yield put(actions.DetailLoaded(result.data));
  } catch (error) {
    yield put(actions.DetailError(error));
  }
}

function* mySaga() {
  yield takeEvery(actionTypes.LOAD_MOVIES, fetchMovies);
  yield takeEvery(actionTypes.LOAD_MOVIES_PAGE, fetchMoviesPage);
  yield takeEvery(actionTypes.LOAD_DETAIL, fetchDetail);
}

export default mySaga;
