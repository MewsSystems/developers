import React from 'react'
import {Col} from 'antd'
import {HeighRow} from 'common/styles'
import RatesHeader from 'components/RatesHeader'
import PairsSelector from 'components/PairsSelector'
import connect from './connect'

class Rates extends React.Component {
	render() {
		return <React.Fragment>
			<HeighRow type="flex" justify="center">
				<Col xs={24} sm={24} md={20} lg={16} xl={15} xxl={14}>
					<RatesHeader>Please, select the rates to display</RatesHeader>
					<PairsSelector
						pairs={this.props.pairs}
						selectedPairs={this.props.selectedPairs}
						selectPairs={this.props.selectPairs}
					/>
				</Col>
			</HeighRow>
		</React.Fragment>
	}
}

export default connect(Rates)