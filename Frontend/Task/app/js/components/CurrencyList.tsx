import { CurrencyPairs } from '../interfaces/CurrencyPairs';
import * as React from 'react';
import { CurrentTrend } from './CurrentTrend';
import Filter from './Filter';
import { connect } from 'react-redux';
import { State } from '../store/store';

const List = ({ pairs, filteredIds, error }: { pairs: CurrencyPairs, error: boolean, filteredIds: string[] }) => {
	let currencyIds = Object.keys(pairs);
	if (filteredIds.length > 0) {
		currencyIds = currencyIds.filter(id => filteredIds.indexOf(id) > -1);
	}

	return <div>
		<Filter />
		<table className="table table-striped"  style={{ fontSize: '24px '}}>
			<thead>
				<tr>
					<th>Code</th>
					<th>Value</th>
					<th>Trend</th>
				</tr>
			</thead>
			<tbody>
				{currencyIds.map((key, index) => <tr key={key} data-id={key}>
					<td>{pairs[key].pair[0].code}/{pairs[key].pair[1].code}</td>
					<td>{pairs[key].value}</td>
					<td><CurrentTrend trend={pairs[key].trend} /></td>
				</tr>)}
			</tbody>
		</table>
		{error === true ? <span className="text-danger">We were not able to update values. Trying again...</span> : ''}
	</div>
}

const mapStateToProps = (state: State) => {
	return {
		filteredIds: state.filteredIds || []
	};
}

export const CurrencyList = connect(
	mapStateToProps
)(List);