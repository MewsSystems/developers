import { Dispatch, SetStateAction } from 'react';

import { PaginationItem } from './PaginationItem';
import css from './pagination.module.css';

import { generatePageNumbers } from '@/utils/generatePageNumbers';

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
  const firstThreePages = pageNumbers.slice(0, 3);
  const lastPage = numberOfPages;

  const increment = () => setCurrentPage((currPage) => currPage + 1);
  const decrement = () => setCurrentPage((currPage) => currPage - 1);

  if (pageNumbers.length <= 3) {
    return (
      <ul className={css.container}>
        <li className={css.button} onClick={decrement}>
          PREVIOUS
        </li>
        {pageNumbers.map((pageNumber) => (
          <PaginationItem
            key={pageNumber}
            page={pageNumber}
            currentPage={currentPage}
            setCurrentPage={setCurrentPage}
          />
        ))}
        <li className={css.button} onClick={increment}>
          NEXT
        </li>
      </ul>
    );
  }

  if (pageNumbers.length === 4) {
    return (
      <ul className={css.container}>
        <li className={css.button} onClick={decrement}>
          PREVIOUS
        </li>
        {firstThreePages.map((pageNumber) => (
          <PaginationItem
            key={pageNumber}
            page={pageNumber}
            currentPage={currentPage}
            setCurrentPage={setCurrentPage}
          />
        ))}
        <PaginationItem
          page={lastPage}
          currentPage={currentPage}
          setCurrentPage={setCurrentPage}
        />
        <li className={css.button} onClick={increment}>
          NEXT
        </li>
      </ul>
    );
  }

  if (pageNumbers.length > 4) {
    return (
      <ul className={css.container}>
        <li className={css.button} onClick={decrement}>
          PREVIOUS
        </li>
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

        {currentPage <= 3 && <div>...</div>}

        {currentPage > 3 && currentPage < lastPage && (
          <>
            {currentPage > firstThreePages[2] + 1 && <div>...</div>}

            <PaginationItem
              page={currentPage}
              currentPage={currentPage}
              setCurrentPage={setCurrentPage}
            />

            {currentPage < lastPage - 1 && <div>...</div>}
          </>
        )}

        {currentPage === lastPage && <div>...</div>}

        <PaginationItem
          page={lastPage}
          currentPage={currentPage}
          setCurrentPage={setCurrentPage}
        />
        <li className={css.button} onClick={increment}>
          NEXT
        </li>
      </ul>
    );
  }
};
