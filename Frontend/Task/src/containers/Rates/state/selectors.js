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
