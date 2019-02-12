import {createSelector} from 'reselect'

const selectAppDomain = state => state.get(`app`)
export const makeSelectConfig = () => createSelector(
	selectAppDomain,
	app => app.get(`config`)
)
export const makeSelectLoading = () => createSelector(
	selectAppDomain,
	app => app.get(`loading`)
)
