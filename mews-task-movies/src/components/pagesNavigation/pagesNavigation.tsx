import './pagesNavigation.css';

export default function PagesNavigation({
  pageNumber,
  totalPagesNumber,
  increasePage,
  decreasePage,
}: {
  pageNumber: number;
  totalPagesNumber: number;
  increasePage: () => void;
  decreasePage: () => void;
}) {
  const diableNextPage = totalPagesNumber === pageNumber;
  const diablePreviousPage = pageNumber === 1;

  return (
    <nav className="page_navigation">
      <button
        className={`${
          diablePreviousPage ? 'disabled button control' : 'button control'
        } `}
        onClick={decreasePage}
        disabled={diablePreviousPage}
      >
        {'<'}
      </button>
      <span className="page_number">
        {pageNumber} / {totalPagesNumber}
      </span>
      <button
        className={`${
          diableNextPage ? 'disabled button control' : 'button control'
        } `}
        onClick={increasePage}
        disabled={diableNextPage}
      >
        {'>'}
      </button>
    </nav>
  );
}
