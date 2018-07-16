// App.js
import React from 'react'
import { hot } from 'react-hot-loader'
import { Provider } from 'react-redux';

import { Button } from 'mews-ui';
import OrderDialog from './billings/OrderDialog';

import './styles.scss';
import 'font-awesome/css/font-awesome.min.css';

import store from './store';

const App = () => (
	<Provider store={store}>
		<div>
			<OrderDialog />
		</div>
	</Provider>
);

export default hot(module)(App);
