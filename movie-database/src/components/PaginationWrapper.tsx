import { Pagination as PaginationSetup, PaginationContent, PaginationItem, PaginationLink, PaginationNext, PaginationPrevious } from './ui/Pagination';

interface Props {
  currentPage: number;
  totalPages: number;
  handlePageChange: (page: number) => void;
}

interface PaginationSetup {
  visibleRange: number[];
  firstPageVisible: boolean;
  lastPageVisible: boolean;
  frontEllipsisVisible: boolean;
  backEllipsisVisible: boolean;
}

const MAX_PAGES = 3;

const PaginationWrapper = ({ currentPage, handlePageChange, totalPages }: Props) => {
  const getPaginationSetup = (): PaginationSetup => {
    const defaultSetup = {
      visibleRange: Array.from({ length: totalPages }, (_, i) => i + 1),
      firstPageVisible: false,
      lastPageVisible: false,
      frontEllipsisVisible: false,
      backEllipsisVisible: false,
    };

    if (totalPages <= MAX_PAGES) {
      return defaultSetup;
    }
    if (currentPage < MAX_PAGES) {
      return {
        ...defaultSetup,
        visibleRange: defaultSetup.visibleRange.slice(0, MAX_PAGES),
        backEllipsisVisible: currentPage + 1 < totalPages,
        lastPageVisible: true,
      };
    }
    if (currentPage >= totalPages - 2) {
      return {
        ...defaultSetup,
        visibleRange: defaultSetup.visibleRange.slice(currentPage - 3, currentPage + 1),
        firstPageVisible: true,
        lastPageVisible: false,
        frontEllipsisVisible: currentPage - 1 > 1,
        backEllipsisVisible: currentPage + 2 < totalPages,
      };
    }
    return {
      visibleRange: defaultSetup.visibleRange.slice(currentPage - 2, currentPage + 1),
      firstPageVisible: true,
      lastPageVisible: true,
      frontEllipsisVisible: currentPage - 1 > 2,
      backEllipsisVisible: currentPage + 2 < totalPages,
    };
  };

  const { visibleRange, firstPageVisible, lastPageVisible, frontEllipsisVisible, backEllipsisVisible } = getPaginationSetup();

  return (
    <PaginationSetup>
      <PaginationContent>
        <PaginationItem>
          <PaginationPrevious onClick={() => handlePageChange(Math.max(currentPage - 1, 1))} />
        </PaginationItem>

        {firstPageVisible && (
          <PaginationItem key={1}>
            <PaginationLink onClick={() => handlePageChange(1)}>1</PaginationLink>
          </PaginationItem>
        )}

        {frontEllipsisVisible && (
          <PaginationItem key={0}>
            <PaginationLink>...</PaginationLink>
          </PaginationItem>
        )}

        {visibleRange.map((page) => (
          <PaginationItem key={page}>
            <PaginationLink isActive={page === currentPage} onClick={() => handlePageChange(page)}>{page}</PaginationLink>
          </PaginationItem>
        ))}

        {backEllipsisVisible && (
          <PaginationItem key={4}>
            <PaginationLink>...</PaginationLink>
          </PaginationItem>
        )}

        {lastPageVisible && (
          <PaginationItem key={totalPages}>
            <PaginationLink onClick={() => handlePageChange(totalPages)}>{totalPages}</PaginationLink>
          </PaginationItem>
        )}

        <PaginationItem>
          <PaginationNext onClick={() => handlePageChange(Math.min(currentPage + 1, totalPages))} />
        </PaginationItem>
      </PaginationContent>
    </PaginationSetup >
  );
};

export default PaginationWrapper;
