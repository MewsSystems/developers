import React from 'react';
import styles from './Pagination.module.css';

interface PaginationProps {
  currentPage: number;
  totalPages: number;
  onNextPage: () => void;
  onPrevPage: () => void;
}

const Pagination: React.FC<PaginationProps> = ({
  currentPage,
  totalPages,
  onNextPage,
  onPrevPage,
}) => {
  return (
    <div className={styles.pagination}>
      <button onClick={onPrevPage} disabled={currentPage === 1}>
        {'<'}
      </button>
      <span>Page {currentPage} of {totalPages}</span>
      <button onClick={onNextPage} disabled={currentPage === totalPages}>
        {'>'}
      </button>
    </div>
  );
};

export default Pagination;
