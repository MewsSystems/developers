import { Chip, IconButton } from "..";
import LastPageIcon from "@material-ui/icons/LastPage";
import FirstPageIcon from "@material-ui/icons/FirstPage";
import NavigateBeforeIcon from "@material-ui/icons/NavigateBefore";
import NavigateNextIcon from "@material-ui/icons/NavigateNext";
import styled from "styled-components";

export interface PaginationProps {
  currentPage: number;
  totalPages: number;
  onChange: (page: number) => void;
}

const StyledWrapper = styled.div`
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 16px;
`;

const ButtonsWrapper = styled.div`
  display: flex;
  gap: 8px;
`;

export function Pagination({ currentPage, totalPages, onChange }: PaginationProps) {
  const canGoNext = currentPage < totalPages;
  const canGoPrev = currentPage > 1;

  const onPrevClick = () => {
    if (currentPage > 1) {
      onChange(currentPage - 1);
    }
  };

  const onNextClick = () => {
    if (currentPage < totalPages) {
      onChange(currentPage + 1);
    }
  };

  const onFirstClick = () => {
    onChange(1);
  };

  const onLastClick = () => {
    onChange(totalPages);
  };

  return (
    <StyledWrapper>
      <ButtonsWrapper>
        <IconButton
          size="small"
          disabled={!canGoPrev}
          onClick={onFirstClick}
          data-testid="first-page-btn"
        >
          <FirstPageIcon />
        </IconButton>
        <IconButton
          size="small"
          disabled={!canGoPrev}
          onClick={onPrevClick}
          data-testid="prev-page-btn"
        >
          <NavigateBeforeIcon />
        </IconButton>
        <IconButton
          size="small"
          disabled={!canGoNext}
          onClick={onNextClick}
          data-testid="next-page-btn"
        >
          <NavigateNextIcon />
        </IconButton>
        <IconButton
          size="small"
          disabled={!canGoNext}
          onClick={onLastClick}
          data-testid="last-page-btn"
        >
          <LastPageIcon />
        </IconButton>
      </ButtonsWrapper>
      <Chip label={currentPage + " of " + totalPages} />
    </StyledWrapper>
  );
}
