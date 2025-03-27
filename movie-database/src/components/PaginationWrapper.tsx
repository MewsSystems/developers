import clsx from 'clsx';
import {
  Pagination,
  PaginationContent,
  PaginationItem,
  PaginationLink,
  PaginationNext,
  PaginationPrevious
} from './ui/Pagination';

interface Props {
  currentPage: number;
  totalPages: number;
  handlePageChange: (page: number) => void;
}

interface PaginationConfig {
  visibleRange: number[];
  firstPageVisible: boolean;
  lastPageVisible: boolean;
  frontEllipsisVisible: boolean;
  backEllipsisVisible: boolean;
}

const MAX_VISIBLE_PAGES = 3;

const PaginationWrapper = ({ currentPage, handlePageChange, totalPages }: Props) => {
  const getPaginationConfig = (): PaginationConfig => {
    const defaultConfig: PaginationConfig = {
      visibleRange: Array.from({ length: totalPages }, (_, i) => i + 1),
      firstPageVisible: false,
      lastPageVisible: false,
      frontEllipsisVisible: false,
      backEllipsisVisible: false,
    };

    if (totalPages <= MAX_VISIBLE_PAGES) {
      return { ...defaultConfig, };
    }

    let startPage = Math.max(currentPage - 1, 1);
    const endPage = Math.min(startPage + MAX_VISIBLE_PAGES - 1, totalPages);

    if (endPage === totalPages) {
      startPage = Math.max(totalPages - MAX_VISIBLE_PAGES + 1, 1);
    }

    return {
      visibleRange: Array.from({ length: endPage - startPage + 1 }, (_, i) => startPage + i),
      firstPageVisible: startPage > 1,
      lastPageVisible: endPage < totalPages,
      frontEllipsisVisible: startPage > 2,
      backEllipsisVisible: endPage < totalPages - 1,
    };
  };

  const { visibleRange, firstPageVisible, lastPageVisible, frontEllipsisVisible, backEllipsisVisible } = getPaginationConfig();

  return (
    <Pagination>
      <PaginationContent>
        <PaginationItem>
          <PaginationPrevious
            onClick={() => handlePageChange(currentPage - 1)}
            className={clsx({ "pointer-events-none opacity-50": currentPage === 1 })} />
        </PaginationItem>

        {firstPageVisible && (
          <PaginationItem>
            <PaginationLink onClick={() => handlePageChange(1)}>1</PaginationLink>
          </PaginationItem>
        )}

        {frontEllipsisVisible && (
          <PaginationItem>
            <PaginationLink className='pointer-events-none'>...</PaginationLink>
          </PaginationItem>
        )}

        {visibleRange.map((page) => (
          <PaginationItem key={page}>
            <PaginationLink
              isActive={page === currentPage}
              onClick={() => handlePageChange(page)}
            >
              {page}
            </PaginationLink>
          </PaginationItem>
        ))}

        {backEllipsisVisible && (
          <PaginationItem>
            <PaginationLink className='pointer-events-none'>...</PaginationLink>
          </PaginationItem>
        )}

        {lastPageVisible && (
          <PaginationItem>
            <PaginationLink onClick={() => handlePageChange(totalPages)}>{totalPages}
            </PaginationLink>
          </PaginationItem>
        )}

        <PaginationItem>
          <PaginationNext
            onClick={() => handlePageChange(currentPage + 1)}
            className={clsx({ "pointer-events-none opacity-50": currentPage === totalPages })}
          />
        </PaginationItem>
      </PaginationContent>
    </Pagination>
  );
};

export default PaginationWrapper;
