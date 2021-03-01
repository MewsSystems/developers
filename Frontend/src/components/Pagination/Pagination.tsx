import { ChevronLeft, ChevronRight } from '@styled-icons/fa-solid';
import { PaginationButton, PaginationContainer } from './styled';
import { generatePageNumSequence } from './utils';

export interface PaginationProps {
  page: number;
  pageCount: number;
  onPageClick: (page: number) => void;
}

const Pagination = ({ page, pageCount, onPageClick }: PaginationProps) => {
  if (pageCount < 2) {
    return null;
  }

  const pageNumSeq = generatePageNumSequence(page, 1, pageCount);

  return (
    <PaginationContainer>
      {page > 1 && (
        <PaginationButton
          type="button"
          variant="secondary"
          onClick={() => onPageClick(page - 1)}
        >
          <ChevronLeft />
        </PaginationButton>
      )}

      {pageNumSeq.map((data) => {
        const isCurrent = data.value === page;
        const isGap = typeof data.value !== 'number';

        return (
          <PaginationButton
            key={data.id}
            type="button"
            variant={isCurrent ? 'primary' : 'secondary'}
            onClick={() =>
              typeof data.value === 'number' && onPageClick(data.value)
            }
            disabled={isCurrent || isGap}
            noPadding={isGap}
          >
            {data.label}
          </PaginationButton>
        );
      })}

      {page < pageCount && (
        <PaginationButton
          type="button"
          variant="secondary"
          onClick={() => onPageClick(page + 1)}
        >
          <ChevronRight />
        </PaginationButton>
      )}
    </PaginationContainer>
  );
};

export default Pagination;
