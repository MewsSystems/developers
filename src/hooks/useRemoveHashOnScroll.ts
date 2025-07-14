import { useEffect } from 'react';

export function useRemoveHashOnScroll() {
  useEffect(() => {
    const removeHash = () => {
      if (window.location.hash) {
        history.replaceState(
          null,
          document.title,
          window.location.pathname + window.location.search
        );
      }
    };

    window.addEventListener('scroll', removeHash, { once: true });
    return () => window.removeEventListener('scroll', removeHash);
  }, []);
}
