/* environmental imports */
import React from 'react'
import thunk from 'redux-thunk'
import ReactDOM from 'react-dom'
import { Provider } from 'react-redux'
import { createStore, applyMiddleware } from 'redux'

/* app imports */
import { endpoint, interval } from './config'
import exchangeReducers from './reducers'
import { ExchangeAppContainer } from './containers'
import { saveStateToStorage, loadStateFromStorage } from './functions'
/* end of imports */

const localState = loadStateFromStorage()

const store = createStore(exchangeReducers, { endpoint, interval, ...localState }, applyMiddleware(thunk)) // assuming we can rely on json loading safely

store.subscribe(() => {
	const { lastAction, rates, ...SaveableState } = store.getState()
	saveStateToStorage({
		...SaveableState
	})
})

ReactDOM.render(
	<Provider store={store}>
		<ExchangeAppContainer />
	</Provider>,
	document.getElementById('exchange-rate-client')
)