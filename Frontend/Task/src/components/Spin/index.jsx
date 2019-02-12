import React from 'react'
import {Col} from 'antd'
import {HeighRow} from 'common/styles'
import {StyledSpin} from './styles'

export default () =>
	<HeighRow type="flex" justify="center" align="middle">
		<Col>
			<StyledSpin tip="Please wait, configuration is loading..." size="large"></StyledSpin>
		</Col>
	</HeighRow>