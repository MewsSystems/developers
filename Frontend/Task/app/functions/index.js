export const calculateTrends = (filters, prevValue, currentValue) => {
	if( !prevValue || !currentValue ) {
		return 'unknown'
	}

	try {
		const result = Object.keys(filters).find((filterName) => {
			return filters[filterName](prevValue, currentValue)
		})
		return result
	} catch (e) {
		console.error('Can\'t apply trending filters:')
		console.error(e)
	}
}

export const loadStateFromStorage = () => {
	try {
		const state = localStorage.getItem('state')
		if (!state) {
			return undefined
		} else {
			return JSON.parse(state)
		}
	} catch (e) {
		console.warn('Couldn\'t load store from the storage')
		return undefined
	}
}


export const saveStateToStorage = (state) => {
  try {
    const serializedState = JSON.stringify(state);
    localStorage.setItem('state', serializedState);
  } catch (e) {
  	console.warn('Unable to save store to localStorage:')
  	console.warn(e)
  }
}