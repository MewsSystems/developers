import { useRouter, useSearchParams } from "next/navigation";
import { useCallback } from "react";

export const useUrlParams = () => {
  const router = useRouter();
  const searchParams = useSearchParams();

  const queryParam = searchParams.get("query") ?? "";
  const pageParam = parseInt(searchParams.get("page") ?? "1", 10);

  const updateUrl = useCallback(
    (query: string, page: number) => {
      const params = new URLSearchParams(searchParams.toString());

      if (query) {
        params.set("query", query);
      } else {
        params.delete("query");
      }

      if (page > 1) {
        params.set("page", page.toString());
      } else {
        params.delete("page");
      }

      router.replace(`?${params.toString()}`, { scroll: false });
    },
    [router, searchParams],
  );

  return { queryParam, pageParam, updateUrl };
};
