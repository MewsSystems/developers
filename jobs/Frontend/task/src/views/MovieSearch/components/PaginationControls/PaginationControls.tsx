import { FC } from "react";
import { IconButton } from "src/components/IconButton/IconButton";
import { ArrowLeft } from "src/components/icons/ArrowLeft";
import { ArrowRight } from "src/components/icons/ArrowRight";
import { InputEnter } from "src/components/InputEnter/InputEnter";
import { PaginationControlsProps } from "src/views/MovieSearch/components/PaginationControls/PaginationControlsProps";
import styled from "styled-components";

export const PaginationControls: FC<PaginationControlsProps> = (props) => {
  if (!props.page) return null;

  return (
    <PaginationWrap data-testid={"pagination"}>
      <IconButton
        name={<ArrowLeft />}
        onClick={() => props.setPage(props.page - 1)}
        disabled={props.page === 1}
      />
      <PageNumberWrap>
        <InputEnter
          onEnter={(pageValue: number) => props.setPage(pageValue)}
          value={props.page}
          inputType={"number"}
        />
        <TotalPageNumber>/{props?.totalPages}</TotalPageNumber>
      </PageNumberWrap>
      <IconButton
        name={<ArrowRight />}
        onClick={() => props.setPage(props.page + 1)}
        disabled={props.page === props.totalPages}
      />
    </PaginationWrap>
  );
};

const PaginationWrap = styled.div`
  display: flex;
  align-items: center;
  align-self: flex-end;
`;

const PageNumberWrap = styled.div`
  display: flex;
  gap: 8px;
`;

const TotalPageNumber = styled.div`
  font-size: 20px;
`;
