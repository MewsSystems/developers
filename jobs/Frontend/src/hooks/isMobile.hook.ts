import { useEffect, useState } from 'react'

export const useIsMobile = () => {
  /*
        Set to 1920 during SSR because there is not window object.
        By using 1920 as default value we assume desktop first design approach,
        resulting in less layout shift (CLS) on desktop.
    */
  const [width, setWidth] = useState(1920)

  const handleWindowSizeChange = () => {
    setWidth(window.innerWidth)
  }

  useEffect(() => {
    if (typeof window === 'undefined') {
      return
    }

    // Update the width immediately the window is available on client side
    setWidth(window.innerWidth)

    window.addEventListener('resize', handleWindowSizeChange)

    return () => {
      window.removeEventListener('resize', handleWindowSizeChange)
    }
  }, [])

  const isMobile = width <= 768
  return { isMobile }
}
