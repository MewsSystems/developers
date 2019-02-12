import {takeLatest, put, call} from 'redux-saga/effects'
import request from 'utils/request'
import {ratesAPIURI} from 'common/constants'
import {SET_CONFIG} from 'containers/App/state/constants'
import {setPairs} from './actions'

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

function *fetchRates() {
	const rates = yield call(request.get, `${ratesAPIURI}/rates`, {currencyPairIds: [`5b428ac9-ec57-513d-8a08-20199469fb4d`]})
	console.log(rates)
}

export default function *storageSaga() {
	yield takeLatest(SET_CONFIG, buildPairs)
}