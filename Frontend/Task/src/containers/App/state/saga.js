import {put, call} from 'redux-saga/effects'
import request from 'utils/request'
import storage from 'utils/local-storage'
import {ratesAPIURI} from 'common/constants'
import {setLoading, setConfig} from './actions'

function *checkConfig() {
	if (`config` in storage) {
		yield put(setLoading(false))
		yield call(loadConfig, storage.config)
	}
	else {
		yield call(fetchConfig)
	}
}

function *fetchConfig() {
	const config = yield call(request.get, `${ratesAPIURI}/configuration`)
	yield put(setLoading(false))

	if (config) {
		yield call(storeConfig, config)
	}
}

function *storeConfig(config) {
	storage.config = config
	yield call(loadConfig, config)
}

function *loadConfig(config) {
	yield put(setConfig(config))
}

export default function *storageSaga() {
	yield call(checkConfig)
}