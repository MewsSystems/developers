import {connect} from 'react-redux'
import {createStructuredSelector} from 'reselect'
import {makeSelectLoading} from './state/selectors'

const props = createStructuredSelector({
	loading: makeSelectLoading(),
})

export default Component => connect(props)(Component)
