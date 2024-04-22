import {
  Pagination,
  PaginationContent,
  PaginationEllipsis,
  PaginationItem,
  PaginationLink,
  PaginationNext,
  PaginationPrevious,
} from "@/design-system/components/ui/pagination";
import { times } from "@/design-system/lib/utils";
import { BaseComponentProps } from "@/types";

export type SearchPaginationProps = BaseComponentProps & {
  currentPage: number;
  totalPages: number;
  linkBuilder: (page: number) => string;
};

const PAGES_TO_NAVIGATE = 3;

export function SearchPagination({
  currentPage,
  totalPages,
  linkBuilder,
  ...props
}: SearchPaginationProps) {
  const from = Math.max(1, currentPage - PAGES_TO_NAVIGATE);
  const to = Math.min(totalPages, currentPage + PAGES_TO_NAVIGATE);
  const pagesToRender = to - from + 1;

  return (
    <Pagination {...props}>
      <PaginationContent>
        {currentPage > 1 && (
          <PaginationItem>
            <PaginationPrevious href={linkBuilder(currentPage - 1)} />
          </PaginationItem>
        )}

        {from > PAGES_TO_NAVIGATE && (
          <PaginationItem>
            <PaginationEllipsis data-testid="pagination-backwards-ellipsis" />
          </PaginationItem>
        )}

        {times(pagesToRender, (i) => {
          const newPage = from + i;
          return (
            <PaginationItem key={newPage}>
              <PaginationLink
                isActive={currentPage === newPage}
                href={linkBuilder(newPage)}
              >
                {newPage}
              </PaginationLink>
            </PaginationItem>
          );
        })}

        {to < totalPages && (
          <PaginationItem>
            <PaginationEllipsis data-testid="pagination-further-ellipsis" />
          </PaginationItem>
        )}

        {currentPage < totalPages && (
          <PaginationItem>
            <PaginationNext href={linkBuilder(currentPage + 1)} />
          </PaginationItem>
        )}
      </PaginationContent>
    </Pagination>
  );
}
