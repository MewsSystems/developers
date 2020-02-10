import React from "react";
import styled, { css } from "styled-components";

const range = (size: number, startAt: number) => {
  return [...Array(size).keys()].map((_, i) => i + startAt);
}

const PaginationContainer = styled.ul`
  list-style-type: 'none';
  padding: 0; 
  display: 'flex'; 
  justify-content: 'space-between';
`;

type ItemProps = {
  active?: boolean,
  disabled?: boolean,
}

export const Item = styled.li<ItemProps>`
  display: inline-block;
  border-radius:2px;
  text-align:center;
  vertical-align:top;
  height:30px;
  font-size:1.2em;
  font-weight:400;
  padding: 0 10px;
  line-height: 30px;
  text-decoration: none;
  color: #444;
  user-select:none;
  ${props => !props.disabled && css`
  &:hover {
    cursor:pointer
  }`}
  ${props => props.active && css`
    background-color:#ee6e73;
    color: #fff;
  `}
  ${props => props.disabled && css`
    color: #ccc;
  `}
  }
`;

type PaginationProps = {
  buttonAmount: number,
  totalPages: number,
  current: number,
  onPageChange: (page: number) => void
}

export const Pagination: React.FC<PaginationProps> = ({ buttonAmount, totalPages, onPageChange, current }) => {
  const isFirstPage = current === 1;
  const isLastPage = current === totalPages;

  if (totalPages === 0) {
    return null
  }

  const onClick = (page: number, canChange: boolean) => {
    if (current === page || !canChange) {
      return;
    }

    onPageChange(page);
  }

  const buttons = buttonAmount < totalPages ? buttonAmount : totalPages;
  const startAt = current === 1 ? 1 :
    current === totalPages ? totalPages - buttons + 1 :
      current - 1;

  return (
    <PaginationContainer>
      <Item
        disabled={isFirstPage}
        onClick={onClick.bind(null, current - 1, !isFirstPage)}>
        <i className="fa fa-chevron-left" />
      </Item>

      {range(buttons, startAt).map(p =>
        <Item key={p} active={current === p} onClick={onClick.bind(null, p, true)}>{p}</Item>
      )}

      <Item
        disabled={isLastPage}
        onClick={onClick.bind(null, current + 1, !isLastPage)}>
        <i className="fa fa-chevron-right" />
      </Item>
    </PaginationContainer>
  )
}
