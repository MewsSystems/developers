import React from 'react';
import styled from 'styled-components';

const Form = styled.form`
  padding: 20px 0 40px 0;
  background-color: #276278;
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
`;

const SearchArea = (props) => {
  return (
    <Form
      onSubmit={(e) => {
        e.preventDefault();
      }}
    >
      <div>
        <input
          placeholder="Search movie"
          type="text"
          onChange={props.handleChange}
        />
      </div>
    </Form>
  );
};

export default SearchArea;
