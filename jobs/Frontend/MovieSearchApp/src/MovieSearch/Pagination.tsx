import { Button } from "@/components/ui/button";
import { ChevronsLeft, ChevronLeftIcon, ChevronRightIcon, ChevronsRight } from "lucide-react";

export type PaginationProps = {
  page: number;
  pageCount: number;
  goToPage: (pageIndex: number) => void;
  isDataLoading: boolean;
};

export function Pagination({ page, pageCount, goToPage, isDataLoading }: PaginationProps) {
  return (
    <div className="flex flex-wrap items-center justify-between py-4">
      <div className="font-medium py-2">
        Page {page} of {pageCount}
      </div>
      <div className="flex flex-wrap items-center gap-2">
        <Button
          variant="outline"
          className="shadow-sm"
          onClick={() => goToPage(1)}
          disabled={!(page > 1) || isDataLoading}
        >
          <span className="sr-only">Go to first page</span>
          <ChevronsLeft className="h-4 w-4 mr-2" />
          First
        </Button>
        <Button
          variant="outline"
          className="shadow-sm"
          onClick={() => goToPage(page - 1)}
          disabled={!(page > 1) || isDataLoading}
        >
          <span className="sr-only">Go to previous page</span>
          <ChevronLeftIcon className="h-4 w-4 mr-2" />
          Previous
        </Button>
        <Button
          variant="outline"
          className="shadow-sm"
          onClick={() => goToPage(page + 1)}
          disabled={!(page < pageCount) || isDataLoading}
        >
          <span className="sr-only">Go to next page</span>
          Next
          <ChevronRightIcon className="h-4 w-4 ml-2" />
        </Button>
        <Button
          variant="outline"
          className="shadow-sm"
          onClick={() => goToPage(pageCount)}
          disabled={!(page < pageCount) || isDataLoading}
        >
          <span className="sr-only">Go to last page</span>
          Last
          <ChevronsRight className="h-4 w-4 ml-2" />
        </Button>
      </div>
    </div>
  );
}
