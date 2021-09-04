import React from 'react';
import styled from 'styled-components';
import { css } from 'styled-components';

const Ul = styled.ul`
  padding: 0;
  display: flex;
  flex-direction: row;
  flex-wrap: wrap;
  justify-content: center;

  margin: 15px auto;

  li {
    list-style: none;
  }

  @media (min-width: 1000px) {
    max-width: 1000px;
  }
`;

const Button = styled.button`
  font-size: 16px;
  margin-right: 10px;
  font-family: 'Roboto', sans-serif;
  border: none;
  background-color: transparent;
  cursor: pointer;

  ${(props) =>
    props.primary &&
    css`
      color: #bd7898;
      font-weight: bold;
    `}

  @media (min-width: 768px) {
    font-size: 16px;
  }
`;

const Pagination = (props) => {
  const pageLinks = [];

  for (let i = 1; i <= props.pages; i += 1) {
    // let active = props.currentPage === i ? 'active' : '';
    // nastavit jako třídu

    pageLinks.push(
      <li key={i} onClick={() => props.nextPage(i)}>
        <Button primary={props.currentPage === i ? true : false} type="button">
          {i}
        </Button>
      </li>,
    );
  }

  return (
    <Ul onClick={() => window.scrollTo(0, 0)}>
      {props.currentPage > 1 ? (
        <li onClick={() => props.nextPage(props.currentPage - 1)}>
          <Button>←</Button>
        </li>
      ) : (
        ''
      )}
      {pageLinks}
      {props.currentPage < props.pages ? (
        <li onClick={() => props.nextPage(props.currentPage + 1)}>
          <Button>→</Button>
        </li>
      ) : (
        ''
      )}
    </Ul>
  );
};

export default Pagination;
