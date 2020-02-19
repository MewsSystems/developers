import { put, call, takeLatest, all } from 'redux-saga/effects';
import { processRequest } from '../services/Api';
import { moviesActionTypes } from './MoviesConstants';
import * as moviesActions from './MoviesActions';
//import { handleError } from '../services/SagasErrorHandler';

const API_PATHNAME = 'carriers';
const FETCH_FIELDS = 'user_packages';

export default function* () {
  yield all([
    yield takeLatest(moviesActionTypes.FETCH_MOVIES, handleGetMoviesRequest),
  ]);
}

export function* handleGetMoviesRequest(action) {
  try {
    const { query } = action.payload;
    const {data} = yield call(processRequest, `search/movie?query=${query}`);
   debugger;

   /* const results = yield all([
      syncColumnsSettingsRequest && call(handleGetColumnsVisibilityRequest, GridActions.getGridColumnsVisibilitySettingsRequest(GRID_NAME)),
      call(processRequest, url),
    ]);*/

    yield put(moviesActions.fetchMoviesSuccess(data));
  } catch(e) {
   // yield call(handleError, e, 'Error during fetching carriers!');
    //yield put(carriersActions.getCarriersError(e));
  }
}
