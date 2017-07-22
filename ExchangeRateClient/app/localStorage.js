export const loadState = () => {
	try {
		const serializableStorage = localStorage.getItem('state');
		if (serializableStorage === null) {
			return undefined;
		}
		return JSON.parse(serializableStorage);
	} catch (err) {
		return undefined;
	}
};

export const saveState = (state) => {
	try {
		const serializableStorage = JSON.stringify(state);
		localStorage.setItem('state', serializableStorage);
	} catch (err) {
		// console.log(err);
		return undefined;
	}
}