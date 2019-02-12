import {connect} from 'react-redux'
import {createStructuredSelector} from 'reselect'
import {makeSelectPairs, makeSelectSelectedPairs} from './state/selectors'
import {selectPairs} from './state/actions'

const props = createStructuredSelector({
	pairs: makeSelectPairs(),
	selectedPairs: makeSelectSelectedPairs(),
})
const actions = {
	selectPairs,
}

export default Component => connect(props, actions)(Component)
