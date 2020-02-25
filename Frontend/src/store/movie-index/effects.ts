import { all, call, fork, put, takeLatest, select } from 'redux-saga/effects'
import { MovieIndexActionTypes } from './types'
import { fetchSearchRequestError, fetchSearchRequestSuccess } from './actions'
import { callApi, API_ENDPOINT, API_KEY } from '../../utils/api'

function getSearchUrl(searchTerm: string, page: number) {
  const seachConcatenated = searchTerm.split(' ').join('+')
  const seachQuery = `query=${seachConcatenated}` // encode?
  const searchUrl = `${API_ENDPOINT}/search/movie?${seachQuery}&page=${page}&api_key=${API_KEY}`
  return searchUrl
}

// eslint-disable-next-line @typescript-eslint/no-explicit-any
function* handleSearchChange(action: any) {
  try {
    if (!action.payload) {
      yield put(fetchSearchRequestSuccess([]))
      return
    }
    const searchUrl = getSearchUrl(action.payload, 1)
    // To call async functions, use redux-saga's `call()`.
    const res = yield call(callApi, 'get', searchUrl)
    if (res.error) {
      yield put(fetchSearchRequestError(res.error))
    } else {
      yield put(fetchSearchRequestSuccess(res))
    }
  } catch (err) {
    if (err instanceof Error && err.stack) {
      yield put(fetchSearchRequestError(err.stack))
    } else {
      yield put(fetchSearchRequestError('An unknown error occured.'))
    }
  }
}

// This is our watcher function. We use `take*()` functions to watch Redux for a specific action
// type, and run our saga, for example the `handleFetch()` saga above.
function* watchSearchChange() {
  yield takeLatest(MovieIndexActionTypes.SEARCH_CHANGED, handleSearchChange)
}

// eslint-disable-next-line @typescript-eslint/no-explicit-any
function* handlePageChange(action: any) {
  try {
    const search = yield select(state => state.movieIndex.search)
    const searchUrl = getSearchUrl(search, action.payload)
    // To call async functions, use redux-saga's `call()`.
    const res = yield call(callApi, 'get', searchUrl)
    if (res.error) {
      yield put(fetchSearchRequestError(res.error))
    } else {
      yield put(fetchSearchRequestSuccess(res))
    }
  } catch (err) {
    if (err instanceof Error && err.stack) {
      yield put(fetchSearchRequestError(err.stack))
    } else {
      yield put(fetchSearchRequestError('An unknown error occured.'))
    }
  }
}

function* watchPageChange() {
  yield takeLatest(MovieIndexActionTypes.PAGE_CHANGED, handlePageChange)
}

// We can also use `fork()` here to split our saga into multiple watchers.
function* movieIndexSaga() {
  yield all([fork(watchSearchChange), fork(watchPageChange)])
}

export default movieIndexSaga
