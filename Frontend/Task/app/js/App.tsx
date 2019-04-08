import * as React from 'react';
import { Spinner } from './components/Spinner';
import { connect } from 'react-redux';
import { State } from './store/store';
import { CurrencyPairs } from './interfaces/CurrencyPairs';
import { CurrencyList } from './components/CurrencyList';

const App = (props: { pairs: CurrencyPairs, error: boolean }) => {
	return <div className="row">
		<div className="col-12 ml-auto mr-auto" style={{ maxWidth: '600px', marginTop: '10vh' }}>
			{Object.keys(props.pairs).length > 0 ? <CurrencyList pairs={props.pairs} error={props.error} /> : <Spinner />}
		</div>
	</div>
}

const mapStateToProps = (state: State) => {
	return {
		pairs: state.pairs,
		error: state.error
	};
}

const Connteced = connect(
	mapStateToProps
)(App);

export default Connteced;