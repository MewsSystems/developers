import styled, { css } from 'styled-components';

export const PaginationWrapper = styled.div`
  display: flex;
  justify-content: space-between;
  flex-wrap: wrap;
  padding: 0 10px;
  width: 100%;
`;

export const PaginationPagesWrapper = styled.div`
  display: flex;
  flex-wrap: wrap;
`;

export const PageItem = styled.div<{ disabled: boolean; current: boolean }>`
  padding: 5px;
  margin: 0 5px;
  background-color: var(--common-color-blue-1);
  border: 1px solid var(--common-color-blue-2);
  border-radius: 5px;
  cursor: pointer;
  font-size: 14px;

  &:hover {
    ${props => !props.disabled && !props.current && css`
      background-color: var(--common-color-blue-4);
    `}
  }

  ${props => props.disabled && css`
    opacity: 0.5;
    cursor: not-allowed;
  `}

  ${props => props.current && css`
    background-color: var(--common-color-blue-3);
    cursor: default;
  `}
`;

export const Dots = styled.div`
  margin: 0 5px;
  align-self: end;
`;