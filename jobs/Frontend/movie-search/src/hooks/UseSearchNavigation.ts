import { usePathname, useRouter, useSearchParams } from "next/navigation";

export const useSearchNavigation = () => {
  const searchParams = useSearchParams();
  const pathname = usePathname();
  const { replace } = useRouter();

  const searchterm = searchParams.get("searchterm") ?? "";
  const currentPage = Number(searchParams.get("page")) || 1;

  const movieSearch = (searchTerm: string, page: number = 1) => {
    const params = new URLSearchParams(searchParams);

    if (searchTerm) {
      params.set("searchterm", searchTerm); 
      params.set("page", page.toString());
    } else {
      params.delete("searchterm");
      params.delete("page");
    }

    replace(`${pathname}?${params.toString()}`);
  };

  return { movieSearch, searchterm, currentPage };
};
