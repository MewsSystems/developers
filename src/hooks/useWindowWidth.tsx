import { useState, useEffect } from 'react';

export const useWindowWidth = () => {
  const [deviceType, setDeviceType] = useState<string | undefined>(getDeviceType());

  function getDeviceType() {
    const windowWidth = window.innerWidth;

    if (windowWidth < 768) {
      return 'mobile';
    } else if (windowWidth < 1024) {
      return 'tablet';
    } else {
      return 'desktop';
    }
  }

  useEffect(() => {
    const handleResize = () => {
      setDeviceType(getDeviceType());
    };
    window.addEventListener('resize', handleResize);

    return () => {
      window.removeEventListener('resize', handleResize);
    };
  }, []);

  return {
    isMobile: deviceType === 'mobile',
    isTablet: deviceType === 'tablet',
    isDesktop: deviceType === 'desktop' || !deviceType,
  };
};
