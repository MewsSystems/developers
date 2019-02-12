import React from 'react'
import ErrorTrapper from 'components/ErrorTrapper'
import Spin from 'components/Spin'
import GlobalStyle, {StyledContent} from './styles'
import connect from './connect'

const App = ({loading}) =>
	<React.Fragment>
		<ErrorTrapper key="error-trapper"/>
		<GlobalStyle/>
		<StyledContent>
			{loading && <Spin/> || <p>rates selector</p>}
		</StyledContent>
	</React.Fragment>

export default connect(App)