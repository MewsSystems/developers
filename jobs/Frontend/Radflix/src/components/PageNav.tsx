import React from "react";
import "../CSS/PageNav.css";

interface PageNavProps {
  pageAmount: number | undefined;
  changePage: (newPage: number) => void;
  page: number | undefined;
}

const PageNav: React.FC<PageNavProps> = ({ pageAmount, changePage, page }) => {
  const totalPages = pageAmount ? pageAmount : 0;

  // Go to the previous page
  const handlePrevPage = () => {
    if (page && page > 1) {
      changePage(page - 1);
    }
  };

  // Go to the next page
  const handleNextPage = () => {
    if (page && page < totalPages) {
      changePage(page + 1);
    }
  };

  // Go to the first page
  const handleFirstPage = () => {
    if (page && page !== 1) {
      changePage(1);
    }
  };

  // Go to the last page
  const handleLastPage = () => {
    if (page && page !== totalPages) {
      changePage(totalPages);
    }
  };

  const renderPaginationButtons = () => {
    const buttons = [];
    // Lowest page
    const startPage = Math.max(
      1,
      Math.min(page! - 4, Math.max(totalPages - 9, 1))
    );
    // Highest page
    const endPage = Math.min(startPage + 9, totalPages);

    // Generate buttons
    for (let index = startPage; index <= endPage; index++) {
      buttons.push(
        <button
          key={index}
          onClick={() => changePage(index)}
          className={index === page ? "active-page" : ""}
        >
          {index}
        </button>
      );
    }

    return buttons;
  };

  return (
    <div className="page-nav-container">
      {page && pageAmount && (
        <>
          <button disabled={page <= 1} onClick={handleFirstPage}>
            &lt;&lt;
          </button>
          <button disabled={page <= 1} onClick={handlePrevPage}>
            &lt;
          </button>
        </>
      )}
      {renderPaginationButtons()}
      {page && pageAmount && (
        <>
          <button disabled={page == totalPages} onClick={handleNextPage}>
            &gt;
          </button>
          <button disabled={page == totalPages} onClick={handleLastPage}>
            &gt;&gt;
          </button>
        </>
      )}
    </div>
  );
};

export default PageNav;
