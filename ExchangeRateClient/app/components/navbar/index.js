import React, { Component } from 'react'
import { Link } from 'react-router-dom';
import { Pane, Text } from 'evergreen-ui';

export default class Navbar extends Component {

	render() {
		return (
			<Pane
				elevation={4}
				width={'100%'}
				height={75}
				display="flex"
				justifyContent="center"
				alignItems="center"
				flexDirection="row"
				marginBottom={15}
			>
				<Link to={'/'}>
					<Text marginRight={12} clearfix>Welcome To My Project</Text>
				</Link>
			</Pane>
		)
	}
}