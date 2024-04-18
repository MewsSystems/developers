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
  return (
    <nav className="page_navigation">
      {pageNumber > 1 && (
        <span className="button control" onClick={decreasePage}>
          {'<'}
        </span>
      )}
      <span className="page_number">
        {pageNumber} / {totalPagesNumber}
      </span>
      {totalPagesNumber > pageNumber && (
        <span className="button control" onClick={increasePage}>
          {'>'}
        </span>
      )}
    </nav>
  );
}
