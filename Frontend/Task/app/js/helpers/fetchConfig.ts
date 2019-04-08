import store, { addConfig } from '../store/store';

const configUrl = 'http://localhost:3000/configuration';

const fetchConfig = () => new Promise((resolve, reject) => {
	fetch(configUrl).then(response => {
		response.json().then(json => {
			resolve(json);
		}).catch(reject);
	}).catch(reject);
});

export const tryFetchConfig = async () => {
	try {
		// Try fetch and save to store
		let config = await fetchConfig();
		store.dispatch(addConfig(config));
		return config;
	} catch (err) {
		// If fails, try again
		console.warn(err);
		return await tryFetchConfig();
	}
}
