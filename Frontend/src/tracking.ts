import ReactGA from 'react-ga'

const initializeTracking = () => {
  ReactGA.initialize(String(process.env.GA_TRACKING_CODE)) // TODO: Add tracking code to env config
}

export default initializeTracking
