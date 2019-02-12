import React from 'react'
import {Col} from 'antd'
import {StyledRow, StyledSpin} from './styles'

export default () =>
	<StyledRow type="flex" justify="center" align="middle">
		<Col>
			<StyledSpin tip="Please wait, configuration is loading..." size="large"></StyledSpin>
		</Col>
	</StyledRow>