import { useCallback } from 'react'
import ReactGA from 'react-ga'
import { useLocation } from 'hooks/useLocation'

export interface TrackEventProps {
  eventCategory: string
  eventAction: string
  eventLabel?: string
  customDimensions?: { [key: string]: string }
}

export const useTrackEvent = () => {
  const {
    location,
    location: { pathname, href },
  } = useLocation()

  const setTrackEvent = useCallback(
    ({
      eventCategory,
      eventAction,
      eventLabel,
      customDimensions,
    }: TrackEventProps) => {
      ReactGA.send({
        page: pathname,
        location: href,
        eventCategory,
        eventAction,
        eventLabel,
        ...customDimensions,
        hitType: 'event',
      })
    },
    [pathname, href]
  )

  return { location, setTrackEvent }
}
