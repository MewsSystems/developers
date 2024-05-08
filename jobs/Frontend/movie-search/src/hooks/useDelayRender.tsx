import { useEffect, useState } from 'react';

/**
 * React hook that delays rendering of content based on a condition and optional delay.
 *
 * @param condition - A boolean value determining whether to render content.
 * @param delay (optional) - The delay in milliseconds before rendering after the condition changes. Defaults to 1000ms.
 * @returns A boolean value indicating whether the content should be rendered based on the condition and delay.
 */
export default function useDelayedRender(condition: boolean, delay = 1000) {
  const [shouldRender, setShouldRender] = useState(false);

  useEffect(() => {
    const timeoutId = setTimeout(() => setShouldRender(condition), delay);
    return () => clearTimeout(timeoutId);
  }, [condition, delay]);

  return shouldRender;
}
