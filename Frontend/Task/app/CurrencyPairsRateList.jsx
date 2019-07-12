import React from 'react';

export default class CurrencyPairsRateList extends React.Component {
	render () {
		const { rateList } = this.props;

		return (<div>
			{!!rateList.length && <div>
				<p>Currency pairs rate list</p>
				<div><span>Currency pair </span><span>Current rate </span><span>Trend </span></div>
				{
					rateList.map(ratePair => (
						<div key={ratePair.pairId}>
							<span>{ratePair.currency} </span>
							<span>{ratePair.rate} </span>
							<span>{ratePair.trend}</span>
						</div>
					))
				}
			</div>}
		</div>)
	}
}
