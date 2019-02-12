import {connect} from 'react-redux'
import {createStructuredSelector} from 'reselect'
import {makeSelectLoading, makeSelectConfig} from './state/selectors'

const props = createStructuredSelector({
	loading: makeSelectLoading(),
	config: makeSelectConfig(),
})

export default Component => connect(props)(Component)
