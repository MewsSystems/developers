import styled from 'styled-components';
import Button from '../common/Button';

interface PaginationButtonProps {
  noPadding?: boolean;
}

export const PaginationButton = styled(Button)<PaginationButtonProps>`
  border: none;
  padding: ${({ theme, noPadding }) =>
    noPadding ? 0 : `${theme.space[3]} ${theme.space[2]}`};

  @media screen and (min-width: ${({ theme }) => theme.breakPoints.tablet}) {
    padding: ${({ theme, noPadding }) =>
      noPadding ? 0 : `${theme.space[3]} ${theme.space[3]}`};
  }

  :disabled {
    cursor: default;
  }

  > svg {
    padding-bottom: ${({ theme }) => theme.space[1]};
    width: ${({ theme }) => theme.fontSizes.s};
    height: ${({ theme }) => theme.fontSizes.s};
  }
`;

export const PaginationContainer = styled.div`
  padding-bottom: ${({ theme }) => theme.space[4]};
  text-align: center;
  white-space: nowrap;
`;
