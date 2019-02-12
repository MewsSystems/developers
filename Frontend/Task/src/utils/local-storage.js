const storage = new Proxy(localStorage, {
	set(target, key, value) {
		target.setItem(key, JSON.stringify(value))
		return true
	},
	get(target, key) {
		const value = target.getItem(key) || null
		return JSON.parse(value)
	}
})

export default storage
