import { takeEvery, call, put, delay, select } from 'redux-saga/effects';
import { configurationSucceeded, rateSucceeded, getRate } from "./actions";
import { host } from "./api";
import { getPairsSelector, getConfiguration } from "./selector";

export function* watchGetConfiguration() {
	yield takeEvery('GET_CONFIGURATION', fetchConfiguration);
}

export function* watchGetRate() {
	yield takeEvery('GET_RATE', fetchGetRate);
}

function* fetchGetRate() {
	try {
		const userPairsSelector = yield select(getPairsSelector);
		const configuration = yield select(getConfiguration);
		let pairsList = [];

		if (userPairsSelector.length) {
			pairsList = [...userPairsSelector]
		} else {
			pairsList = Object.keys(configuration);
		}

		const data = yield call(() => fetch(`${host}/rates?currencyPairIds=${JSON.stringify(pairsList)}`).then(res => res.json()));
		const currencyPairsRateList = [];

		if (data) {
			const rates = data.rates || {};

			for (let rate in rates) {
				currencyPairsRateList.push({pairId: rate, rate: rates[rate], pairs: configuration[rate]})
			}

			yield put(rateSucceeded(currencyPairsRateList));
		}
	} catch (error) {
		console.log(`Error getting rate, ${error}`);
		location.reload();
	}
}

function* fetchConfiguration () {
	try {
		const configuration = yield call(() => fetch(`${host}/configuration`).then(res => res.json()));
		yield put(configurationSucceeded(configuration));
		yield put(getRate());
	} catch (error) {
		console.log(`Error getting config, ${error}`);
	}
}

