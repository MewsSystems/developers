import React from "react";
import { Button } from "./button";
import styled from "styled-components";

// Returns configuration array. -1 means dots item

const getPaginationConfigList = (
  maxPages: number,
  currentPage: number
): number[] => {
  const result = [];
  if (maxPages > 0 && maxPages < 3) {
    for (let i = 1; i <= maxPages; i++) {
      result.push(i);
    }
  } else {
    const startPaginationPage = 1;
    const middlePaginationPage =
      currentPage === 1 || currentPage === maxPages
        ? Math.ceil(maxPages / 2)
        : currentPage;
    result.push(startPaginationPage, -1, middlePaginationPage, -1, maxPages);
  }
  return result;
};

interface IPaginationProps {
  maxPages: number;
  currentPage: number;
  onNextPageClick: () => void;
  onPreviousPageClick: () => void;
  onExactPageClick: (page: number) => void;
}

export const Pagination: React.FC<IPaginationProps> = (props) => {
  const {
    maxPages,
    currentPage,
    onNextPageClick,
    onPreviousPageClick,
    onExactPageClick,
  } = props;

  if (maxPages === 0) {
    return null;
  }

  const paginationConfigList = getPaginationConfigList(maxPages, currentPage);
  return (
    <PaginationContainer>
      <Button
        onClick={onPreviousPageClick}
        content="<"
        variant="secondary"
        isDisabled={currentPage === 1}
      />
      {(paginationConfigList.length === 2 &&
        paginationConfigList.map((pageNumber) => {
          const handleButtonClick = () => {
            onExactPageClick(pageNumber);
          };

          return (
            <Button
              onClick={handleButtonClick}
              content={pageNumber.toString()}
              variant={pageNumber === currentPage ? "active" : "secondary"}
              isDisabled={currentPage === maxPages}
            />
          );
        })) ||
        paginationConfigList.map((pageNumber) => {
          const shouldRenderDots = pageNumber === -1;
          if (shouldRenderDots) {
            return <Dots>...</Dots>;
          }
          const handleButtonClick = () => {
            onExactPageClick(pageNumber);
          };
          return (
            <Button
              onClick={handleButtonClick}
              content={pageNumber.toString()}
              variant={pageNumber === currentPage ? "active" : "secondary"}
              isDisabled={false}
            />
          );
        })}
      <Button
        onClick={onNextPageClick}
        content=">"
        variant="secondary"
        isDisabled={true}
      />
    </PaginationContainer>
  );
};

const PaginationContainer = styled.div`
  width: max-content;
  display: flex;
  padding: ${(props) => props.theme.offset["0.5"]};
`;

const Dots = styled.div`
  width: 2rem;
  height: 2rem;
  text-align: center;
  padding-top: ${(props) => props.theme.offset["0.5"]};
  color: ${(props) => props.theme.color.darkBlue};
`;
