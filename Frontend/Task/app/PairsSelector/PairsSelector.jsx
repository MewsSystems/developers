import React from 'react';
import './PairSelector.scss';

export default class PairsSelector extends React.Component {
	handleChange = (e) => {
		if (this.props.onChange) {
			this.props.onChange(e.target.value);
		}
	};

	render () {
		const { pairs } = this.props;

		return (
			<div>
				<h3>Pairs to select</h3>
				{pairs.map(item => {
					return <div key={item.key} className='pair'>
						<label
							htmlFor={item.key}>
							<input type="checkbox"
							       className="check-hidden"
							       id={item.key}
							       checked={item.selected}
							       name="pair"
							       value={item.key}
							       hidden
							       onChange={this.handleChange}
							/>
							<span className="check-mark"></span>
							<span className="code">{item.pair[0].code}</span>
							{item.pair[0].name} -
							<span className="code">{item.pair[1].code}</span>
							{item.pair[1].name}</label>
					</div>
				})}
			</div>)
	}
}
