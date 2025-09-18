import { useEffect, useRef } from "react";

type UseInfiniteScrollProps = {
  hasNextPage: boolean | undefined;
  isFetchingNextPage: boolean;
  fetchNextPage: () => void;
};

export function useInfiniteScroll({ hasNextPage, isFetchingNextPage, fetchNextPage }: UseInfiniteScrollProps) {
  const ref = useRef<HTMLDivElement | null>(null);

  useEffect(() => {
    const el = ref.current;
    if (!el || !hasNextPage || isFetchingNextPage) return;

    const obs = new IntersectionObserver((entries) => {
      if (entries[0]?.isIntersecting) fetchNextPage();
    }, { rootMargin: "200px" });

    obs.observe(el);
    return () => obs.disconnect();
  }, [hasNextPage, isFetchingNextPage, fetchNextPage]);

  return ref;
}
