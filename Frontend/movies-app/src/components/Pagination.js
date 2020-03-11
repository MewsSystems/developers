import React from 'react';
import { Button } from './styled';

const LEFT_PAGE = '__LEFT__';
const RIGHT_PAGE = '__RIGHT__';

const range = (from, to, step = 1) => {
  let i = from;
  const range = [];

  while (i <= to) {
    range.push(i);
    i += step;
  }

  return range;
};

const fetchPageNumbers = (currentPage, totalPages, pageNeighbours) => {
  /**
   * totalNumbers: the total page numbers to show on the control
   * totalBlocks: totalNumbers + 2 to cover for the left(<) and right(>) controls
   */
  const totalNumbers = pageNeighbours * 2 + 3;
  const totalBlocks = totalNumbers + 2;

  if (totalPages > totalBlocks) {
    const startPage = Math.max(2, currentPage - pageNeighbours);
    const endPage = Math.min(totalPages - 1, currentPage + pageNeighbours);

    let pages = range(startPage, endPage);

    /**
     * hasLeftSpill: has hidden pages to the left
     * hasRightSpill: has hidden pages to the right
     * spillOffset: number of hidden pages either to the left or to the right
     */
    const hasLeftSpill = startPage > 2;
    const hasRightSpill = totalPages - endPage > 1;
    const spillOffset = totalNumbers - (pages.length + 1);

    switch (true) {
      // handle: (1) < {5 6} [7] {8 9} (10)
      case hasLeftSpill && !hasRightSpill: {
        const extraPages = range(startPage - spillOffset, startPage - 1);
        pages = [LEFT_PAGE, ...extraPages, ...pages];
        break;
      }

      // handle: (1) {2 3} [4] {5 6} > (10)
      case !hasLeftSpill && hasRightSpill: {
        const extraPages = range(endPage + 1, endPage + spillOffset);
        pages = [...pages, ...extraPages, RIGHT_PAGE];
        break;
      }

      // handle: (1) < {4 5} [6] {7 8} > (10)
      case hasLeftSpill && hasRightSpill:
      default: {
        pages = [LEFT_PAGE, ...pages, RIGHT_PAGE];
        break;
      }
    }

    return [1, ...pages, totalPages];
  }

  return range(1, totalPages);
};

const Pagination = ({ currentPage, totalPages, onPageChanged = page => {}, left = '<', right = '>' }) => {
  return (
    <>
      {fetchPageNumbers(currentPage, totalPages, 2).map(page => {
        if (page === LEFT_PAGE) {
          return (
            <Button
              key={page}
              onClick={() => {
                onPageChanged(currentPage - 1);
              }}
            >
              {left}
            </Button>
          );
        } else if (page === RIGHT_PAGE) {
          return (
            <Button
              key={page}
              onClick={() => {
                onPageChanged(currentPage + 1);
              }}
            >
              {right}
            </Button>
          );
        } else if (page === currentPage) {
          return (
            <Button key={page} disabled>
              {page}
            </Button>
          );
        } else {
          return (
            <Button
              key={page}
              onClick={() => {
                onPageChanged(page);
              }}
            >
              {page}
            </Button>
          );
        }
      })}
    </>
  );
};

export default Pagination;
