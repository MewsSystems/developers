import React, { useMemo } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import styled, { css } from 'styled-components';
import { animateScroll as scroll } from 'react-scroll';
import { RootState } from '../../redux/store';
import { setPage } from '../../redux/searchSlice';

const Pager = styled.ul`
  display: flex;
  flex-wrap: wrap;
  align-items: baseline;
  justify-content: center;
  margin-bottom: 60px;
  
  li {
    padding: 4px;
  }
`;

const PageButton = styled.button`
  background: transparent;
  border: 1px solid #eee;
  border-radius: 4px;
  padding: 8px 12px;
  transition: border-color .2s;
   
  ${(props: { active: boolean }) => props.active && css`
    background-color: #eee;
    font-weight: 700;
    pointer-events: 'none';
  `}
  
  ${(props: { active: boolean }) => !props.active && css`
    cursor: pointer;
    
    &:hover,
    &:active,
    &:focus {
      border-color: #999;
    }
  `}
`;

export default function Pagination() {
  const { currentPage, totalPages } = useSelector((state: RootState) => state.search);
  const dispatch = useDispatch();

  function handlePageClick(count: number) {
    dispatch(setPage(count));

    scroll.scrollTo(200, {
      smooth: true,
    });
  }

  const pageList = useMemo(() => {
    const tempPageList = [];
    let insertDots = false;

    for (let count = 1; count <= totalPages; count++) {
      if (totalPages < 5 || count === 1 || count === totalPages || count === currentPage
        || count === currentPage - 1 || count === currentPage + 1) {
        tempPageList.push(
          <li key={count}>
            <PageButton type="button" active={count === currentPage} onClick={() => { handlePageClick(count); }}>
              {count}
            </PageButton>
          </li>,
        );

        insertDots = true;
      } else {
        if (insertDots) {
          tempPageList.push(
            <li key={count}>...</li>,
          );
        }

        insertDots = false;
      }
    }

    return tempPageList;
  }, [totalPages, currentPage]);

  return (
    <div>
      {totalPages > 1 && pageList
        && (
          <Pager>
            {pageList}
          </Pager>
        )}
    </div>
  );
}
