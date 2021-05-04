import React from "react";
import { Button } from "./button";
import styled from "styled-components";

interface IPaginationProps {
  maxPages: number;
  currentPage: number;
  onNextPageClick: () => void;
  onPreviousPageClick: () => void;
  onExactPageClick: (page: number) => void;
}

const getPaginationConfigList = (maxPages: number): number[] => {
  const result = [];
  if (maxPages > 0 && maxPages < 3) {
    for (let i = 1; i <= maxPages; i++) {
      result.push(i);
    }
  } else {
    const startPaginationPage = 1;
    const middlePaginationPage = Math.ceil(maxPages / 2);
    result.push(startPaginationPage, -1, middlePaginationPage, -1, maxPages);
  }
  return result;
};

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
  const paginationConfigList = getPaginationConfigList(maxPages);

  if (paginationConfigList.length === 2) {
    return (
      <PaginationContainer>
        <Button
          onClick={onPreviousPageClick}
          content="<"
          variant="secondary"
          isDisabled={false}
        />

        {paginationConfigList.map((pageNumber) => {
          const shouldRenderDots = pageNumber === -1;
          if (shouldRenderDots) {
            return <MultiPaginationItem>...</MultiPaginationItem>;
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
  }
  // for all pages (pages>2)
  return (
    <PaginationContainer>
      <Button
        onClick={onPreviousPageClick}
        content="<"
        variant="secondary"
        isDisabled={false}
      />

      {paginationConfigList.map((pageNumber) => {
        const shouldRenderDots = pageNumber === -1;
        if (shouldRenderDots) {
          return <MultiPaginationItem>...</MultiPaginationItem>;
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
        isDisabled={false}
      />
    </PaginationContainer>
  );
};

const PaginationContainer = styled.div`
  width: max-content;
  display: flex;
  padding: ${(props) => props.theme.offset["0.5"]};
`;

const MultiPaginationItem = styled.div`
  width: 32px;
  height: 32px;
  text-align: center;
  padding-top: ${(props) => props.theme.offset["0.5"]};
  color: ${(props) => props.theme.color.darkBlue};
`;
