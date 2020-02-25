import { all, call, fork, put, takeEvery } from 'redux-saga/effects'
import { MovieInfoActionTypes } from './types'
import { fetchInfoRequestError, fetchInfoRequestSuccess } from './actions'
import { callApi, API_ENDPOINT, API_KEY } from '../../utils/api'

// eslint-disable-next-line @typescript-eslint/no-explicit-any
function* handleFetch(action: any) {
  try {
    // To call async functions, use redux-saga's `call()`.
    const url = `${API_ENDPOINT}/movie/${action.payload}?api_key=${API_KEY}`
    const res = yield call(callApi, 'get', url)

    if (res.error) {
      yield put(fetchInfoRequestError(res.error))
    } else {
      yield put(fetchInfoRequestSuccess(res))
    }
  } catch (err) {
    if (err instanceof Error && err.stack) {
      yield put(fetchInfoRequestError(err.stack))
    } else {
      yield put(fetchInfoRequestError('An unknown error occured.'))
    }
  }
}

// This is our watcher function. We use `take*()` functions to watch Redux for a specific action
// type, and run our saga, for example the `handleFetch()` saga above.
function* watchFetchRequest() {
  yield takeEvery(MovieInfoActionTypes.FETCH_REQUEST, handleFetch)
}

// We can also use `fork()` here to split our saga into multiple watchers.
function* movieInfoSaga() {
  yield all([fork(watchFetchRequest)])
}

export default movieInfoSaga
