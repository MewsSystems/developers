import React from 'react'
import ErrorTrapper from 'components/ErrorTrapper'
import GlobalStyle from './styles'

const App = () => <React.Fragment>
	<ErrorTrapper key="error-trapper"/>
	<GlobalStyle/>
	<p>rates client</p>
</React.Fragment>

export default App