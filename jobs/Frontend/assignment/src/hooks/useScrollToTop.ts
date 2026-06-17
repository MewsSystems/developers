import { useEffect } from "react";

// eslint-disable-next-line @typescript-eslint/no-explicit-any
export function useScrollToTop(dependency: any, smooth: boolean = true) {
  useEffect(() => {
    window.scrollTo({
      top: 0,
      behavior: smooth ? "smooth" : "auto",
    });
  }, [dependency, smooth]);
}
