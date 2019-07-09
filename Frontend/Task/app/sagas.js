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
		
		console.log(configuration);
		console.log(pairsList);

	} catch (error) {
		console.log(`Error getting rate, ${error}`);
	}
}

function* fetchConfiguration () {
	try {
		const data = yield call(() => fetch(`${host}/configuration`).then(res => res.json()));
		yield put(configurationSucceeded(data));
		yield put(getRate());
	} catch (error) {
		console.log(`Error getting config, ${error}`);
	}
}

