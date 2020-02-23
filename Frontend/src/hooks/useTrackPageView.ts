import ReactGA from 'react-ga'
import { useLocation } from 'hooks/useLocation'

export const useTrackPageView = () => {
  const {
    location,
    location: { pathname, href },
  } = useLocation()

  const setTrackPageView = () => {
    ReactGA.send({
      page: pathname,
      location: href,
      hitType: 'pageview',
    })
  }

  return { location, setTrackPageView }
}
