import React from 'react'
import {Statistic, Icon, Row} from 'antd'
import {StyledCard} from './styles'

const getPrefix = (current, last) => {
	if (!current || !last) return undefined
	if (current > last) return <Icon type="like" style={{color: `green`}}/>
	if (current < last) return <Icon type="dislike" style={{color: `red`}}/>

	return <Icon type="dash"/>
}

export default ({rates}) => {
	const statistics = rates.size > 0
		? rates.toJS().map(({pair, current, last}, key) =>
			<StyledCard key={key}>
				<Statistic title={pair.shortName} value={current} prefix={getPrefix(current, last)}/>
			</StyledCard>
		)
		: null

	return <Row type="flex" justify="center">
		{statistics}
	</Row>
}