import classNames from 'classnames';
import { Dispatch, SetStateAction } from 'react';

import css from './paginationItem.module.css';

interface Props {
  page: number;
  currentPage: number;
  setCurrentPage: Dispatch<SetStateAction<number>>;
}

export const PaginationItem = ({
  page,
  currentPage,
  setCurrentPage,
}: Props) => {
  const isActive = page === currentPage;

  return (
    <li className={classNames(css.pageNumber, isActive && css.active)}>
      <span onClick={() => setCurrentPage(page)}>{page}</span>
    </li>
  );
};
