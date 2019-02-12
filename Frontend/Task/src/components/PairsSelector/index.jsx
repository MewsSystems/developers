import React from 'react'
import {Select, Col} from 'antd'
import {MarginRow} from 'common/styles'

const Option = Select.Option

export default props => {
	const pairs = props.pairs ? props.pairs.toJS() : {}
	const options = Object.keys(pairs).map(key =>
		<Option key={key} value={key} title={pairs[key].name}>
			{pairs[key].name}
		</Option>)

	return <MarginRow type="flex" justify="center">
		<Col span={24}>
			<Select size="large" mode="multiple"
				optionFilterProp="title"
				placeholder="Multiple rate pairs are available"
				allowClear
				style={{width: `100%`}}
				onChange={props.selectPairs}
				value={props.selectedPairs.toJS()}
			>
				{options}
			</Select>
		</Col>
	</MarginRow>
}