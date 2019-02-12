import React from 'react'
import {Alert} from 'antd'
import ErrorTrapper from 'components/ErrorTrapper'
import Spin from 'components/Spin'
import Rates from 'containers/Rates'
import GlobalStyle, {StyledContent} from './styles'
import connect from './connect'

const getComponent = (loading, config) => {
	if (loading) return <Spin/>

	return config
		? <Rates/>
		: <Alert
			message="Error loading configuration"
			description="Some error occured while loading configuration."
			type="error"
			showIcon
		/>
}

const App = ({loading, config}) =>
	<React.Fragment>
		<ErrorTrapper key="error-trapper"/>
		<GlobalStyle/>
		<StyledContent>
			{getComponent(loading, config)}
		</StyledContent>
	</React.Fragment>

export default connect(App)