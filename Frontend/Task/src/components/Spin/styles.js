import styled from 'styled-components'
import {Spin} from 'antd'

export const StyledSpin = styled(Spin)`
	> span.ant-spin-dot {
		font-size: 60px;
		width: 60px;
		height: 60px;

		i {
			width: 22px;
			height: 22px;
		}
	}

	> .ant-spin-text {
		@media only screen and (min-width: 600px) {
			font-size: 60px;
		}
		@media only screen and (max-width: 600px) {
			font-size: 32px;
		}
		max-width: 900px;
		text-align: center;
		color: rgba(0, 0, 0, 0.65);
		font-weight: 600;
	}
`