import React from 'react'
import {notification} from 'antd'
import {notifications} from 'common/constants'

export default class ErrorTrapper extends React.PureComponent {
	constructor(props) {
		super(props)

		this.state = {
			code: 0,
			message: ``,
			stack: ``,
		}
	}
	componentDidMount() {
		window.onerror = (...args) => {
			const error = args[4] || {}

			this.setState({
				code: 1,
				message: error.message || args[0],
				stack: error.stack,
			})
		}
	}
	clearError() {
		this.setState({code: 0})
	}
	displayError(hasError) {
		if (!hasError) return null

		const {code, message, stack} = this.state
		const config = {
			description: `${stack || ``}`,
			duration: notifications.error.duration,
			message: `${code || ``} - ${message}`,
			placement: `topLeft`,
			onClose: () => this.clearError()
		}

		notification.error(config)
		return null
	}
	render() {
		const hasError = this.state.code > 0
		return this.displayError(hasError)
	}
}
