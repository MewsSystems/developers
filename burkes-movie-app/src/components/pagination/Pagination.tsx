import { Dispatch, SetStateAction } from 'react';

import { generatePageNumbers } from '@/utils/generatePageNumbers';

import { PaginationItem } from './PaginationItem';
import css from './pagination.module.css';

interface Props {
  numberOfPages: number;
  currentPage: number;
  setCurrentPage: Dispatch<SetStateAction<number>>;
}

export const Pagination = ({
  numberOfPages,
  currentPage,
  setCurrentPage,
}: Props) => {
  const pageNumbers = generatePageNumbers(numberOfPages);
  const lastPage = numberOfPages;

  const handleIncrement = () => {
    if (currentPage === lastPage) return;
    setCurrentPage((currPage) => currPage + 1);
  };

  const handleDecrement = () => {
    if (currentPage === 1) return;
    setCurrentPage((currPage) => currPage - 1);
  };

  return (
    <ul className={css.container}>
      <li className={css.button} onClick={handleDecrement}>
        PREVIOUS
      </li>

      {pageNumbers.length <= 4 &&
        pageNumbers.map((pageNumber) => (
          <PaginationItem
            key={pageNumber}
            page={pageNumber}
            currentPage={currentPage}
            setCurrentPage={setCurrentPage}
          />
        ))}

      {pageNumbers.length > 4 && (
        <>
          <PaginationItem
            page={1}
            currentPage={currentPage}
            setCurrentPage={setCurrentPage}
          />
          <PaginationItem
            page={2}
            currentPage={currentPage}
            setCurrentPage={setCurrentPage}
          />
          <PaginationItem
            page={3}
            currentPage={currentPage}
            setCurrentPage={setCurrentPage}
          />

          {(currentPage <= 3 || currentPage === lastPage) && <div>...</div>}

          {currentPage > 3 && currentPage < lastPage && (
            <>
              {currentPage > 4 && <div>...</div>}

              <PaginationItem
                page={currentPage}
                currentPage={currentPage}
                setCurrentPage={setCurrentPage}
              />

              {currentPage < lastPage - 1 && <div>...</div>}
            </>
          )}

          <PaginationItem
            page={lastPage}
            currentPage={currentPage}
            setCurrentPage={setCurrentPage}
          />
        </>
      )}

      <li className={css.button} onClick={handleIncrement}>
        NEXT
      </li>
    </ul>
  );
};
