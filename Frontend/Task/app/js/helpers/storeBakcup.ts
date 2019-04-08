import store, { State } from '../store/store';

const key = 'currency-settings';

export const storeBackuper = () => {
	if (!window.localStorage) return;
	store.subscribe(() => {
		let state = store.getState();
		localStorage.setItem(key, JSON.stringify(state));
	});
}

export const storeRestore = (): State => {
	let rawStore = localStorage.getItem(key)
	if (rawStore) {
		try {
			let store = JSON.parse(rawStore);
			return store;
		} catch(err) {
			console.warn(err);
		}
	}
}