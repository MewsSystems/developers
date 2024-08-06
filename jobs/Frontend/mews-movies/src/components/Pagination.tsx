import React from "react";
import { PaginationProps } from "../types/PaginationInterfaces";
import styled from "styled-components";

const ButtonElement = styled.button`
  padding: 0.5rem 1rem;
  margin: 0.5rem;
  border-radius: 0.25rem;
  background-color: #4b83f1;
  border: none;
  color: white;
  font-size: 1rem;
  cursor: pointer;
  &:hover {
    background-color: #5ca0ff;
  }
`;

const Pagination: React.FC<PaginationProps> = ({
  page,
  totalPages,
  onNextPage,
  onPrevPage,
}) => {
  return (
    <div>
      {page > 1 && <ButtonElement onClick={onPrevPage}>Previous</ButtonElement>}
      {page < totalPages && (
        <ButtonElement onClick={onNextPage}>Next</ButtonElement>
      )}
    </div>
  );
};

export default Pagination;
