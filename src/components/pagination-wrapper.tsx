import {
  Pagination,
  PaginationContent,
  PaginationEllipsis,
  PaginationItem,
  PaginationNext,
  PaginationPrevious,
  PaginationLink,
} from "~/components/ui/pagination";
import type { Movie } from "~/types/movie";

interface PaginationWrapperProps {
  page: number;
  totalPages: number;
  onPageChange: (page: number) => void;
  moviesToDisplay: Movie[];
}

export function PaginationWrapper({
  page,
  totalPages,
  onPageChange,
  moviesToDisplay,
}: PaginationWrapperProps) {
  const getLinkClass = (isActive: boolean) =>
    isActive
      ? "bg-primary/30 text-white border border-primary min-w-[3.5rem] px-2 flex justify-center items-center"
      : "border border-transparent hover:bg-primary/10 hover:border-primary/30 min-w-[3.5rem] px-2 flex justify-center items-center cursor-pointer";

  const generatePaginationItems = () => {
    const items = [];
    const maxVisiblePages = 3;
    const siblingCount = Math.floor(maxVisiblePages / 2);

    const showLeftEllipsis = page > siblingCount + 2;
    const showRightEllipsis = page < totalPages - siblingCount - 1;

    // Always show "1"
    items.push(
      <PaginationItem key={1}>
        <PaginationLink
          onClick={() => onPageChange(1)}
          className={getLinkClass(page === 1)}
        >
          1
        </PaginationLink>
      </PaginationItem>,
    );

    // Left ellipsis
    if (showLeftEllipsis) {
      items.push(
        <PaginationItem key="start-ellipsis">
          <PaginationEllipsis className="mx-2" />
        </PaginationItem>,
      );
    }

    // Center pages
    const startPage = Math.max(2, page - siblingCount);
    const endPage = Math.min(totalPages - 1, page + siblingCount);

    for (let i = startPage; i <= endPage; i++) {
      items.push(
        <PaginationItem key={i}>
          <PaginationLink
            onClick={() => onPageChange(i)}
            className={getLinkClass(page === i)}
          >
            {i}
          </PaginationLink>
        </PaginationItem>,
      );
    }

    // Right ellipsis
    if (showRightEllipsis) {
      items.push(
        <PaginationItem key="end-ellipsis">
          <PaginationEllipsis className="mx-2" />
        </PaginationItem>,
      );
    }

    // Always show "Last Page"
    if (totalPages > 1) {
      items.push(
        <PaginationItem key={totalPages}>
          <PaginationLink
            onClick={() => onPageChange(totalPages)}
            className={getLinkClass(page === totalPages)}
          >
            {totalPages}
          </PaginationLink>
        </PaginationItem>,
      );
    }

    return items;
  };

  return (
    <>
      {moviesToDisplay.length > 0 && totalPages > 1 && (
        <Pagination className="my-4">
          <PaginationContent className="gap-2">
            <PaginationItem>
              <PaginationPrevious
                onClick={() => onPageChange(Math.max(1, page - 1))}
                className={
                  page <= 1
                    ? "pointer-events-none opacity-50"
                    : "cursor-pointer hover:border hover:border-primary/30 hover:bg-primary/30 hover:text-white"
                }
                aria-disabled={page <= 1}
                aria-label="Go to previous page"
              />
            </PaginationItem>

            {generatePaginationItems()}

            <PaginationItem>
              <PaginationNext
                onClick={() => onPageChange(Math.min(totalPages, page + 1))}
                className={
                  page >= totalPages
                    ? "pointer-events-none opacity-50"
                    : "cursor-pointer hover:border hover:border-primary/30 hover:bg-primary/30 hover:text-white"
                }
                aria-disabled={page >= totalPages}
                aria-label="Go to next page"
              />
            </PaginationItem>
          </PaginationContent>
        </Pagination>
      )}
    </>
  );
}
