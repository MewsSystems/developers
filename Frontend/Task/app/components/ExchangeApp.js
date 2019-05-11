import React from 'react'
import propTypes from 'prop-types'
import Loader from './Loader'
import ExchangeTable from './ExchangeTable'
import {A} from '../actions'
import loadStateFromStorage from '../functions'
import Filters from '../containers/Filters'

import styles from './css/ExchangeApp.scss'

class ExchangeApp extends React.Component {
	constructor (props) {
		super(props)
		this.state = {
			rates: {} /* Saving them in case if we fail to receive data from the server */
		}
	}

	componentDidMount () {
		this.props.initiateExchange()
	}

	componentDidUpdate (prevProps) {
		if (this.props.lastAction == A.GOT_CONFIG ) { // fire an immediate update on config update if the rates are empty to avoid empty look
			this.props.getRates()
		}
		if (this.props.lastAction == A.GOT_RATES || this.props.lastAction == A.FAILED_RATES) {
			this.updateRatesPostponed()
		}
	}

	static getDerivedStateFromProps (props, state) {
		if (props.lastAction !== A.FAILED_RATES) {
			return {
				...state,
				rates: props.rates
			}			
		}

		return null
	}

	updateRatesPostponed () {
		setTimeout(() => {
			this.props.getRates()
		}, this.props.interval)
	}

	render () {
		const { rates } = this.state
		const { currencyPairs, activeFilters } = this.props
		return (
			<div className={styles.container}>
			{
				Object.keys(currencyPairs).length ?
				<div>
					<h1>Cool Currency Rates</h1>
					<ExchangeTable rates={rates} activeFilters={activeFilters} pairs={currencyPairs} />
					<Filters />
				</div> :
				<Loader />
			}
			</div>
		)
	}
}

export default ExchangeApp