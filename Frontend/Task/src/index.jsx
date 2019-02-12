import React from 'react'
import ReactDOM from 'react-dom'
import {Provider} from 'react-redux'
import configureStore from 'store'
import App from 'containers/App'

const store = configureStore()
const MOUNT_NODE = document.getElementById(`exchange-rate-client`)

ReactDOM.render(
	<Provider store={store}>
		<App/>
	</Provider>,
	MOUNT_NODE
)