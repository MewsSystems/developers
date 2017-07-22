import React from 'react';

const TOGGLE_PAIR = 'PAIR.TOGGLE';

export class CurrencyFilter extends React.Component {
	render() {
		const {store}  = this.props;
		return(
		<ul>
			{this.props.list.map( (pair,index) => 
				<li key={"currency-" + pair.id}>
					<label>
						<input type="checkbox" value="1" checked={pair.selected} onChange={() => store.dispatch({type: TOGGLE_PAIR, index: index})} />
						{pair.pair[0].code}/{pair.pair[1].code}
					</label>
				</li>)}
		</ul>)
	}
}