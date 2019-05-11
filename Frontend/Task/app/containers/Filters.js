import {A, fetchConfig, fetchRates, setFilter} from '../actions'
import { connect } from 'react-redux'
import Filters from '../components/Filters'

const MapStateProps = (state, ownProps) => {
	return {
		trendCheckers: state.trendCheckers,
		activeFilters: state.activeFilters
	}
}
const MapDispatchProps = (dispatch, ownProps) => {
	return {
		onChange: (e) => {
			const input = e.target
			dispatch( setFilter(input.value, input.checked) )
		}
	}
}

const FiltersContainer = connect(MapStateProps, MapDispatchProps)(Filters)

export default FiltersContainer

