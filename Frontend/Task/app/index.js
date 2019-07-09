import React from "react";
import ReactDOM from "react-dom";
import {Provider, connect} from 'react-redux';
import "babel-polyfill";

import store from './store';
import App from './App.jsx';

const ConnectedApp = connect((state) => {
	// console.log(state);
	return state;
})(App);

ReactDOM.render(
	<Provider store={store}>
		<ConnectedApp />
	</Provider>,
	document.getElementById('app')
);

//
// class HelloMessage extends React.Component {
// 	render() {
// 		return <div>Hello {this.props.name}</div>;
// 	}
// }
//
// var mountNode = document.getElementById("app");
// ReactDOM.render(<HelloMessage name="Jane" />, mountNode);

