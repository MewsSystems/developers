import { useEffect, useRef } from "react";

type LoadMoreSentinelProps = {
  onLoadMore: () => void;
};

export const LoadMoreSentinel: React.FC<LoadMoreSentinelProps> = ({
  onLoadMore,
}) => {
  const sentinelRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    if (!sentinelRef.current) {
      return;
    }

    const loadMoreObserver = new IntersectionObserver(
      (entries) => {
        entries.forEach((entry) => {
          if (entry.isIntersecting) {
            onLoadMore();
          }
        });
      },
      {
        rootMargin: "5px",
        threshold: 0.5,
      }
    );

    loadMoreObserver.observe(sentinelRef.current);

    return () => {
      loadMoreObserver.disconnect();
    };
  }, [onLoadMore]);

  return <div ref={sentinelRef} />;
};
