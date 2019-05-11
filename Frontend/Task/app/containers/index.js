import {A, fetchConfig, fetchRates} from '../actions'
import { connect } from 'react-redux'
import ExchangeApp from '../components/ExchangeApp'
import Filters from '../components/Filters'

const MapStateProps = (state) => {
	return state
}

const MapDispatchProps = (dispatch) => {
	return {
		initiateExchange: () => { dispatch(fetchConfig()) },
		getRates: () => { dispatch(fetchRates()) }
	}
}


export const ExchangeAppContainer = connect(MapStateProps, MapDispatchProps)(ExchangeApp)