import {connect} from 'react-redux'
import {createStructuredSelector} from 'reselect'
import {
	makeSelectPairs,
	makeSelectSelectedPairs,
	makeSelectPreparedRates,
} from './state/selectors'
import {selectPairs} from './state/actions'

const props = createStructuredSelector({
	pairs: makeSelectPairs(),
	selectedPairs: makeSelectSelectedPairs(),
	rates: makeSelectPreparedRates()
})
const actions = {
	selectPairs,
}

export default Component => connect(props, actions)(Component)
