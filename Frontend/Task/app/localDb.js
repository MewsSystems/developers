
export const currencyLocalDB = {
	get (key) {
		const item = localStorage.getItem(key);
		return item ? JSON.parse(item) : undefined;
	},
	set (key, value) {
		localStorage.setItem(key, JSON.stringify(value))
	}
};
