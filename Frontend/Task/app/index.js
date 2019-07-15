import React from "react";
import ReactDOM from "react-dom";
import {Provider, connect} from 'react-redux';
import "babel-polyfill";

import store from './store/store';
import App from './App/App.jsx';

const ConnectedApp = connect((state) => {
	return state;
})(App);

ReactDOM.render(
	<Provider store={store}>
		<ConnectedApp />
	</Provider>,
	document.getElementById('app')
);
