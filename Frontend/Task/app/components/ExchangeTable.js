import React from 'react'
import ExchangeItem from './ExchangeItem'
import styles from './css/ExchangeTable.scss'

const ExchangeTable = ({rates, pairs, activeFilters}) => { /* consider getting data directly from the store */
	let placeholder = 'Loading...'
	return (
		<div className={styles.container}>
			<table className={styles.table}>
				<thead>
					<tr>
						<th>Currencies</th>
						<th>Ratio</th>
						<th>Trend</th>
					</tr>
				</thead>
				<tbody>
				{
					Object.keys(pairs).map(
						(p,i) => {
							if (rates[p] && activeFilters.indexOf(rates[p].trend) < 0) {
								return (
									<ExchangeItem key={i} styles={styles} rate={rates[p].value} pair={pairs[p]} trend={rates[p].trend} />							
								)							
							}
							else if (!rates[p]) {
								return (
									<ExchangeItem key={i} styles={styles} rate={placeholder} pair={pairs[p]} trend={placeholder} />							
								)
							}
						}
					)
				}
				</tbody>
			</table>
		</div>
	)
}

export default ExchangeTable