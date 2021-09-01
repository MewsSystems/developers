import React from 'react';
import styled from 'styled-components';

const Ul = styled.ul`
  padding: 0;
  display: flex;
  flex-direction: row;
  flex-wrap: wrap;
  justify-content: center;
  width: 80%;
  margin: 15px auto;

  li {
    list-style: none;
  }
`;

const Button = styled.a`
  font-size: 13px;
  margin-right: 13px;
`;

const Pagination = (props) => {
  const pageLinks = [];

  for (let i = 1; i <= props.pages; i += 1) {
    // let active = props.currentPage === i ? 'active' : '';
    // nastavit jako třídu

    pageLinks.push(
      <li key={i} onClick={() => props.nextPage(i)}>
        <Button href="#">{i}</Button>
      </li>,
    );
  }

  return (
    <Ul>
      {props.currentPage > 1 ? (
        <li onClick={() => props.nextPage(props.currentPage - 1)}>
          <Button href="#">← Prev</Button>
        </li>
      ) : (
        ''
      )}
      {pageLinks}
      {props.currentPage < props.pages ? (
        <li onClick={() => props.nextPage(props.currentPage + 1)}>
          <Button href="#">Next →</Button>
        </li>
      ) : (
        ''
      )}
    </Ul>
  );
};

export default Pagination;
