import React from 'react';

const Pagination = ({ currentPage, totalPages, paginate }) => {
  const maxPagesToShow = 2; 
  const totalPagesToShow = Math.min(totalPages, maxPagesToShow);
  const pageNumbers = Array.from({ length: totalPagesToShow }, (_, index) => index + 1);

  return (
    <nav>
      <ul className="pagination justify-content-center">
        {pageNumbers.map((number) => (
          <li key={number} className={`page-item ${number === currentPage ? 'active' : ''}`}>
            <a
              onClick={(e) => {
                e.preventDefault();
                paginate(number);
              }}
              href="#"
              className="page-link"
            >
              {number}
            </a>
          </li>
        ))}
      </ul>
    </nav>
  );
};

export default Pagination;
