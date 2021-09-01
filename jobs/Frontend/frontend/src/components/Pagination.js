import React from 'react';
import styled from 'styled-components';

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
  margin-right: 13px;
  font-family: 'Roboto', sans-serif;
  border: none;
  background-color: transparent;
  cursor: pointer;

  &:hover {
    color: ${(props) => (props.primary ? '#bd7898' : 'black')};
  }

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
        <Button primary type="button" as="a" href="#">
          {i}
        </Button>
      </li>,
    );
  }

  return (
    <Ul>
      {props.currentPage > 1 ? (
        <li onClick={() => props.nextPage(props.currentPage - 1)}>
          <Button href="#">←</Button>
        </li>
      ) : (
        ''
      )}
      {pageLinks}
      {props.currentPage < props.pages ? (
        <li onClick={() => props.nextPage(props.currentPage + 1)}>
          <Button href="#">→</Button>
        </li>
      ) : (
        ''
      )}
    </Ul>
  );
};

export default Pagination;
