import randomString from 'randomstring'

// fetching
export const ratesAPIURI = `http://localhost:3000`

// moment
export const LOCAL_DATE_FORMAT = `L`
export const DATE_FORMAT = `DD.MM.YYYY`
export const DATE_TIME_FORMAT = `DD.MM.YYYY HH:mm:ss`

// notifications
export const notifications = {
	success: {
		duration: 5,
	},
	info: {
		duration: 5,
	},
	warning: {
		duration: 15,
	},
	error: {
		duration: 30,
	},
}

// for test suits
const string = randomString.generate()
export const testValues = {
	string,
	array: [string],
}