import React from "react";
import styled from "styled-components";
import arrow from "./arrow.svg";

const Button = styled.button`
  position: fixed;
  bottom: 20px;
  right: 20px;
  padding: 2vmin;
  background-color: #ff6a00;
  color: white;
  border: none;
  border-radius: 50%;
  cursor: pointer;
  transition: background-color 0.3s ease;
  width: 8vmin;
  height: 8vmin;
  display: ${(props) => (props.show ? "block" : "none")};

  &:hover {
    background-color: #0056b3;
  }

  & > img {
    width: 4vmin;
    height: 4vmin;
  }
`;

export const ScrollToTopButton = ({ show, onClick }) => (
  <Button onClick={onClick} show={show} title="Scroll to Top">
    <img src={arrow} alt="Scroll to top" />
  </Button>
);
