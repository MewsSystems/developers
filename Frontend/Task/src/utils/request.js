import {notification} from 'antd'

const toString = query => {
	const toJSON = param => typeof param === `object` ? JSON.stringify(param) : param
	const urify = param => toJSON(param)
	return Object.keys(query)
		.map(key => `${urify(key)}=${encodeURIComponent(urify(query[key]))}`)
		.join(`&`)
}

const checkStatus = response => {
	if (response.status < 400) {
		return response
	}

	throw response
}

const parseJSON = response => {
	if ([204, 205, 304].includes(response.status)) {
		return null
	}
	return response.json()
}

const errorHandler = async response => {
	const type = response.status < 500 ? `warning` : `error`
	const config = {
		description: `${await response.text() || ``}`,
		duration: null,
		message: `${response.headers.ErrorSource || ``} ${response.status} - ${response.statusText}`,
		placement: `topLeft`
	}

	notification[type](config)

	return null
}

const request = method => (url, params) => {
	let uri = `/${url}`
	const opts = {
		headers: {
			'Accept': `application/json`,
			'Content-Type': `application/json`
		},
		credentials: `same-origin`,
		method
	}
	if (/get|delete/.test(method) && params) {
		uri = `/${url}?${toString(params)}`
	}
	if (/post|put/.test(method)) {
		opts.body = JSON.stringify(params)
	}
	return fetch(uri, opts)
		.then(checkStatus)
		.then(parseJSON)
		.catch(errorHandler)
}

export default {
	get: request(`get`),
	put: request(`put`),
	post: request(`post`),
	delete: request(`delete`)
}
