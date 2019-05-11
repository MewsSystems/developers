import React from 'react'

const ExchangeItem = ({rate, pair, trend, styles}) => {
	const currencyFrom = pair[0]
	const currencyTo = pair[1]
	return (
		<tr>
			<td>
				<span title={currencyFrom.name}>{currencyFrom.code}</span>
				/
				<span title={currencyTo.name}>{currencyTo.code}</span>
			</td>
			<td>
				{rate}
			</td>
			<td>
				<span className={`fas ${styles[trend]}`}>
					{trend}
					<i></i>
				</span>
			</td>
		</tr>
	)
}

export default ExchangeItem