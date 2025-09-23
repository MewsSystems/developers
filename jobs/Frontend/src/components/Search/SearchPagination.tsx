import { MovieSearchResponse } from "src/types/custom";
import {
  Pagination,
  PaginationContent,
  PaginationItem,
  PaginationLink,
  PaginationNext,
  PaginationPrevious,
} from "../ui/pagination";
import { useSearchParams } from "react-router-dom";

export const SearchPagination = ({ data }: { data: MovieSearchResponse }) => {
  let [searchParams, setSearchParams] = useSearchParams();

  const currentPage = parseInt(searchParams.get("page")!);
  const totalPages = data?.total_pages;

  const previousActive = currentPage > 1;
  const nextActive = currentPage < totalPages;

  const pages = getPages(totalPages, currentPage);

  return (
    <Pagination className="bg-gray-300">
      <PaginationContent>
        <PaginationItem tabIndex={0}>
          <PaginationPrevious
            isActive={previousActive}
            onClick={() => {
              if (currentPage > 1)
                setSearchParams(
                  (prev) => {
                    const newParams = new URLSearchParams(prev);
                    newParams.set("page", (currentPage - 1).toString());
                    return newParams;
                  },
                  { replace: false }
                );
            }}
          ></PaginationPrevious>
        </PaginationItem>

        {pages.map(
          ({ displayValue, isSelectable, pageNumber, pageIndex }, index) => (
            <PaginationItem key={pageNumber ?? "ellipsis" + index + 1}>
              {pageNumber === currentPage}
              <PaginationLink
                isActive={pageNumber === currentPage}
                onClick={() =>
                  isSelectable &&
                  setSearchParams(
                    (prev) => {
                      const newParams = new URLSearchParams(prev);
                      newParams.set("page", pageNumber?.toString()!);
                      return newParams;
                    },
                    { replace: false }
                  )
                }
              >
                {displayValue}
              </PaginationLink>
            </PaginationItem>
          )
        )}

        <PaginationItem tabIndex={0}>
          <PaginationNext
            isActive={nextActive}
            onClick={() => {
              if (currentPage < totalPages)
                setSearchParams(
                  (prev) => {
                    const newParams = new URLSearchParams(prev);
                    newParams.set("page", (currentPage + 1).toString());
                    return newParams;
                  },
                  { replace: false }
                );
            }}
          />
        </PaginationItem>
      </PaginationContent>
    </Pagination>
  );
};

const pageNumberToObject = (pageNumber: number) => ({
  displayValue: pageNumber.toString(),
  pageNumber,
  pageIndex: pageNumber - 1,
  isSelectable: true,
});

const getPages = (
  totalPages: number,
  currentPage: number
): {
  displayValue: string;
  isSelectable: boolean;
  pageNumber?: number;
  pageIndex?: number;
}[] => {
  const placeholder = {
    displayValue: "_",
    isSelectable: false,
  };

  if (totalPages <= 6) {
    return Array.from(Array(totalPages).keys()).map((key) =>
      pageNumberToObject(key + 1)
    );
  }

  if (currentPage <= 3 || currentPage >= totalPages - 2) {
    return [
      ...[1, 2, 3].map(pageNumberToObject),
      placeholder,
      ...[totalPages - 2, totalPages - 1, totalPages].map(pageNumberToObject),
    ];
  }

  return [
    ...[1, 2].map(pageNumberToObject),
    placeholder,
    pageNumberToObject(currentPage),
    placeholder,
    ...[totalPages - 1, totalPages].map(pageNumberToObject),
  ];
};
