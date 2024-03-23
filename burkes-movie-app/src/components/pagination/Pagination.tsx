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

  const increment = () => setCurrentPage((currPage) => currPage + 1);
  const decrement = () => setCurrentPage((currPage) => currPage - 1);

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
};
