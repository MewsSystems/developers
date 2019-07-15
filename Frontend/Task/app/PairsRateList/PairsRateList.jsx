import React from 'react';
import './PairRateList.scss';

export default class PairsRateList extends React.Component {
	render () {
		const { rateList } = this.props;

		return (<div>
			{!!rateList.length && <div>
				<h3>Currency pairs rate list</h3>
				<div className='title'>
					<span className='rate'>Pair</span>
					<span className='rate'>Rate</span>
					<span className='rate'>Trend</span>
				</div>
				{
					rateList.map(ratePair => (
						<div key={ratePair.pairId}>
							<span className='rate'>{ratePair.currency} </span>
							<span className='rate'>{ratePair.rate} </span>
							<span className='rate'>{ratePair.trend}</span>
						</div>
					))
				}
			</div>}
		</div>)
	}
}
