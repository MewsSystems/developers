import React from 'react';

export default class PairsSelector extends React.Component {
	handleChange = (e) => {
		if (this.props.onChange) {
			this.props.onChange(e.target.value);
		}
	};

	render () {
		const { currencyPairs, rateList } = this.props;
		console.log(rateList);

		return (
			<fieldset>
				<legend>Currency pairs to select</legend>

				{currencyPairs.map(item => {
					return <div key={item.key}>
						<input type="checkbox" id={item.key} name="currency" value={item.key} onChange={this.handleChange}/>
						<label
							htmlFor={item.key}>{`${item.pair[0].code} ${item.pair[0].name} -
							${item.pair[1].code} ${item.pair[1].name} `}</label><br/>
					</div>
				})}
			</fieldset>)
	}
}
