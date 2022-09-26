import ReactPaginate from "react-paginate";
import styled from "styled-components";
import { mq } from "~/features/ui/theme/mq";
import { palette } from "~/features/ui/theme/palette";

export const StyledPagination = styled(ReactPaginate)`
  all: unset;
  list-style: none;
  display: flex;
  flex-direction: row;
  align-items: center;
  justify-content: center;
  gap: 0.3rem;
  margin-bottom: 7rem;

  & li a {
    border: 1px solid ${palette.grey[800]};
    border-radius: 4px;
    padding: 0.3rem 0.3rem;
    cursor: pointer;

    &:hover {
      background-color: ${palette.grey[800]};
    }
  }

  & .selected > a {
    background-color: ${palette.grey[800]};
  }

  ${mq.medium} {
    gap: 0.5rem;

    & li a {
    padding: 0.3rem 0.5rem;
    }
  }
`