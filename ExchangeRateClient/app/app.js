import React from 'react';
import ReactDOM from 'react-dom';
import { endpoint, interval } from './config';

import { Context } from './context';
import { BrowserRouter, Route } from 'react-router-dom';

import Navbar from './components/navbar/index';
import Homepage from './pages/homepage/index';

export default class App extends React.Component {

	render() {
		return (
			<BrowserRouter>
				<Context>
					<React.Fragment>
						<Navbar />
						<Route path="/" component={Homepage} />
						{/* <Route path="/" component={Homepage} /> */}
						{/* <Route path="/" component={Homepage} /> */}

					</React.Fragment>
				</Context>
			</BrowserRouter>
		)
	}
}

ReactDOM.render(<App />, document.getElementById('exchange-rate-client'));
