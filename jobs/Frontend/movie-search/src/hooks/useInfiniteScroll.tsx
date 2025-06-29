import { useEffect, type RefObject } from "react";

export function useInfiniteScroll(
  ref: RefObject<HTMLDivElement | null>,
  loadMore: () => void,
  hasMore: boolean,
  isLoading: boolean
) {
  useEffect(() => {
    if (!hasMore || isLoading) return;

    const observer = new IntersectionObserver(([entry]) => {
      if (entry.isIntersecting) {
        loadMore();
      }
    });

    const el = ref && ref.current;
    if (el) observer.observe(el);

    return () => observer.disconnect();
  }, [hasMore, isLoading, loadMore, ref]);
}
