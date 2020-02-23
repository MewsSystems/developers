import ReactGA from 'react-ga'

const initializeTracking = () => {
  ReactGA.initialize(String(window._envConfig.GA_TRACKING_CODE))
}

export default initializeTracking
