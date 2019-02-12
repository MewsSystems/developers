import {createStore, applyMiddleware, compose} from 'redux'
import {fromJS} from 'immutable'
import createSagaMiddleware from 'redux-saga'
import reducers from './reducers'
import sagas from './sagas'

const sagaMiddleware = createSagaMiddleware()
const initialState = {}

export default function configureStore() {
	const middlewares = [
		sagaMiddleware,
	]
	const enhancers = [
		applyMiddleware(...middlewares)
	]
	const composeEnhancers =
		process.env.NODE_ENV !== `production` &&
		typeof window === `object` &&
		window.__REDUX_DEVTOOLS_EXTENSION_COMPOSE__
			? window.__REDUX_DEVTOOLS_EXTENSION_COMPOSE__({shouldHotReload: false})
			: compose

	const store = createStore(
		reducers,
		fromJS(initialState),
		composeEnhancers(...enhancers)
	)

	sagaMiddleware.run(sagas)

	return store
}
