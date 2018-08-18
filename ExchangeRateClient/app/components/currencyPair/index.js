import React, { Component } from 'react'
import { withContext } from '../../context';
import { Card, Text, Icon, Tooltip } from 'evergreen-ui';

class CurrencyPair extends Component {
	constructor(props) {
		super(props);
		this.state = {
			icon: null,
			color: null
		}
		this.rate.bind(this);
	}
	rate() {
		const { pair } = this.props;
		const { interval, getRate } = this.props.context
		getRate(pair, (oldRate, newRate) => {
			if (oldRate == newRate) {
				this.setState({
					icon: "symbol-square",
					color: 'warning'
				})
			} else if (oldRate > newRate) {
				this.setState({
					icon: "symbol-triangle-down",
					color: 'danger'
				})
			} else if (oldRate < newRate) {
				this.setState({
					icon: "symbol-triangle-up",
					color: 'success'
				})
			}
			setTimeout(() => {
				this.rate();
			}, interval)
		});
	}
	componentDidMount() {
		this.rate();
	}

	render() {
		const { icon, color } = this.state;
		const { info, pair, context } = this.props;
		return (
			<Tooltip content={
				<Card style={{ textAlign: 'center' }} backgroundColor="overlay">
					<Text color="#F5F6F7">{info[0].name}</Text>
					<Text color="#F5F6F7" clearfix> - </Text>
					<Text color="#F5F6F7">{info[1].name}</Text>
				</Card>
			}>
				<Card
					elevation={1}
					float="left"
					backgroundColor="white"
					width={200}
					height={120}
					margin={24}
					display="flex"
					justifyContent="center"
					alignItems="center"
					flexDirection="column"
				>
					<Text>{info[0].code} -  {info[1].code}</Text>
					<Text>
						{context[`Rate-${pair}`]} 	{icon && <Icon icon={icon} color={color} />}
					</Text>
				</Card>
			</Tooltip>
		);
	}
}
export default withContext(CurrencyPair);