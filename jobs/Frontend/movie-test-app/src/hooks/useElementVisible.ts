import { useCallback, useRef, useState } from 'react';

export const useElementVisible = () => {
  const [visible, setVisible] = useState(false);
  const observer = useRef<IntersectionObserver>();
  const setRef = useCallback((node: HTMLDivElement | null) => {
    if (!node) return;
    if (observer.current) observer.current.disconnect();
    observer.current = new IntersectionObserver(([entry]) => {
      setVisible(entry.isIntersecting);
    });
    if (node) observer.current?.observe(node);
  }, []);

  return { visible, setRef };
};
