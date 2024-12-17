import { useEffect } from "react";
import useDebounce from "@/hooks/useDebounce";

const useInfiniteScroll = (fetchData: () => void) => {
  const handleScroll = () => {
    if (
      document.body.scrollHeight - 300 <
      window.scrollY + window.innerHeight
    ) {
      fetchData();
    }
  };

  const debounceScroll = useDebounce(handleScroll, 500);

  useEffect(() => {
    window.addEventListener("scroll", debounceScroll);

    return () => {
      window.removeEventListener("scroll", debounceScroll);
    };
  }, [debounceScroll]);
};

export default useInfiniteScroll;
