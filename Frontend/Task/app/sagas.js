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

		console.log('pairsList', pairsList);

		const data = yield call(() => fetch(`${host}/rates?currencyPairIds=${JSON.stringify(pairsList)}`).then(res => {console.log('res', res); ; res.json()}));
		const currencyPairsRateList = [];

		if (data) {
			const rates = data.rates || {};

			for (let rate in rates) {
				currencyPairsRateList.push({pairId: rate, rate: rates[rate], pairs: configuration[rate]})
			}

			yield put(rateSucceeded(currencyPairsRateList));

			// console.log(configuration);
			// // console.log(`${host}/rates?currencyPairIds=${JSON.stringify(pairsList)}`);
			// console.log(pairsList);
			// console.log(rates);
			// console.log(currencyPairsRateList);

		}
	} catch (error) {
		console.log(`Error getting rate, ${error}`);
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

