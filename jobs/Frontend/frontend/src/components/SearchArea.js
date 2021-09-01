import React from 'react';
import styled from 'styled-components';

const Form = styled.form`
  padding: 10px 0 40px 0;
  background-color: #bd7898;
  margin-bottom: 40px;

  div {
    width: 80%;
    margin: 0 auto;
  }

  input {
    font-family: 'Montserrat', sans-serif;
    font-size: 16px;
    border-radius: 20px;
    border: none;
    padding: 5px 5%;
    width: 90%;
  }

  @media (min-width: 768px) {
    input {
      padding: 10px 5%;
    }
  }

  @media (min-width: 1000px) {
    div {
      max-width: 1000px;
    }
  }
`;

const SearchArea = (props) => {
  return (
    <Form
      onSubmit={(e) => {
        e.preventDefault();
      }}
    >
      <div>
        <input placeholder="Search" type="text" onChange={props.handleChange} />
      </div>
    </Form>
  );
};

export default SearchArea;
