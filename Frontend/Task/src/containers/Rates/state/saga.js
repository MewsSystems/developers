import {takeLatest, put, call} from 'redux-saga/effects'
import request from 'utils/request'
import {ratesAPIURI} from 'common/constants'
import {SET_CONFIG} from 'containers/App/state/constants'
// import {setLoading, setConfig} from './actions'

function *buildPairs({payload: config}) {
	console.log(config)
}

export default function *storageSaga() {
	yield takeLatest(SET_CONFIG, buildPairs)
}