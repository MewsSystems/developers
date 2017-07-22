export const TOGGLE_PAIR = 'PAIR.TOGGLE';
export const RATE_NEW = 'RATE.NEW';
export const CURRENCY_PAIR_NEW = 'PAIR.NEW';

const checkRate = (newRate, oldRate) => {
	if (newRate > oldRate)
		return "growing"
	else if (newRate < oldRate)
		return "declining"
	else if (newRate == oldRate)
		return "stagnating"
	else
		return null
}


export const currencyRatesStore = (state = [], action) => {
	switch(action.type) {
		case CURRENCY_PAIR_NEW:
			return [...state, {id: action.id, pair: action.pair, selected: action.selected}]
		case RATE_NEW:
			let st = [...state]
				state.map( (item, index) => {
					if (item.id === action.id) {
						st = [...state.slice(0, index),
							{...item, rate: action.rate, old_rate: item.rate, trend: checkRate(action.rate, item.rate)},
	    					...state.slice(index+1)]
					}
				});
				return st;

		case TOGGLE_PAIR:
			state[action.index].selected = !state[action.index].selected
		    return [
		    	...state.slice(0,action.index),
		    	state[action.index],
		    	...state.slice(action.index+1)]
		    	break;
		default: return state;
	}}
