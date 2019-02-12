import {Map} from 'immutable'
import {createSelector} from 'reselect'

const selectRatesDomain = state => state.get(`rates`)
export const makeSelectPairs = () => createSelector(
	selectRatesDomain,
	rates => rates.get(`pairs`)
)
export const makeSelectSelectedPairs = () => createSelector(
	selectRatesDomain,
	rates => rates.get(`selectedPairs`)
)

const makeSelectRates = () => createSelector(
	selectRatesDomain,
	rates => rates.get(`rates`)
)
const makeSelectHistory = () => createSelector(
	selectRatesDomain,
	rates => rates.get(`ratesHistory`)
)
const makeSelectLastHistory = () => createSelector(
	makeSelectHistory(),
	history => history.get(history.size - 2)
)
export const makeSelectPreparedRates = () => createSelector(
	makeSelectRates(),
	makeSelectLastHistory(),
	makeSelectSelectedPairs(),
	makeSelectPairs(),
	(current, last, ids, pairs) => ids.map(id => Map({
		current: current.get(id),
		last: last ? last.get(id) : null,
		pair: pairs.get(id),
	}))
)
