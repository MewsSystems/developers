import { globalHistory } from '@reach/router'
import { useEffect, useState } from 'react'

export const useLocation = () => {
  const initialState = {
    location: globalHistory.location,
    navigate: globalHistory.navigate,
  }

  const [state, setState] = useState(initialState)

  useEffect(() => {
    const removeListener = globalHistory.listen(({ location }) => {
      setState({ ...initialState, ...{ location } })
    })

    return () => {
      removeListener()
    }
  }, [initialState])

  return state
}
