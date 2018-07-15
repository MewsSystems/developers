// App.js
import React from 'react'
import { hot } from 'react-hot-loader'

import { Button } from 'mews-ui';
import OrderDialog from './billings/OrderDialog';

import './styles.scss';
import 'font-awesome/css/font-awesome.min.css';

const App = () => (
	<div>
		<OrderDialog />
	</div>
);

export default hot(module)(App);
