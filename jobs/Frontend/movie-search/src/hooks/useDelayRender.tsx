import { useEffect, useState } from 'react';

export default function useDelayedRender(condition: boolean, delay = 1000) {
  const [shouldRender, setShouldRender] = useState(false);

  useEffect(() => {
    const timeoutId = setTimeout(() => setShouldRender(condition), delay);
    return () => clearTimeout(timeoutId);
  }, [condition, delay]);

  return shouldRender;
}
