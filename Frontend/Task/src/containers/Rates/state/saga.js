import {takeLatest, put, call, delay} from 'redux-saga/effects'
import request from 'utils/request'
import {ratesAPIURI} from 'common/constants'
import {SET_CONFIG} from 'containers/App/state/constants'
import {setPairs, setRates} from './actions'
import {SELECT_PAIRS} from './constants'

const prepareName = (source, full) => source && `${source.code}${full ? `(${source.name})` : ``}`
const getName = (data, full = true) => {
	const name1 = prepareName(data[0], full)
	const name2 = prepareName(data[1], full)

	return `${name1}/${name2}`
}

function *buildPairs({payload: {currencyPairs}}) {
	const pairs = {}

	Object.keys(currencyPairs).forEach(id => {
		pairs[id] = {
			name: getName(currencyPairs[id]),
			shortName: getName(currencyPairs[id], false),
		}
	})

	yield put(setPairs(pairs))
}

function *fetchRates({payload: currencyPairIds}) {
	if (currencyPairIds.length === 0) return

	yield delay(500)
	const rates = yield call(request.get, `${ratesAPIURI}/rates`, {currencyPairIds})

	if (rates) {
		yield put(setRates(rates.rates))	
	}
}

export default function *storageSaga() {
	yield takeLatest(SET_CONFIG, buildPairs)
	yield takeLatest(SELECT_PAIRS, fetchRates)
}