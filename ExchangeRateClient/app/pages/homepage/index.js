import React, { Component } from 'react';
import { Pane, Text } from 'evergreen-ui';

import { withContext } from '../../context';
import CurrencyPair from '../../components/currencyPair';

class Homepage extends Component {
	componentDidMount() {
	}
	render() {
		const { context: { pairs } } = this.props;
		if (!pairs) {
			return (
				<Pane clearfix >
					<Text> Loading ....</Text>
				</Pane>
			)
		}
		return (
			<Pane
				style={{
					display: 'flex',
					flexWrap: 'wrap'
				}}
				justifyContent="center"
				alignItems="center"
				flexDirection="row"
			>
				{Object.keys(pairs).map((pair) => (<CurrencyPair key={pair} pair={pair} info={pairs[pair]} />))}
			</Pane>
		)
	}
}

export default withContext(Homepage);