import styled, { css } from 'styled-components';

export const SearchWrapper = styled.div`
  display: flex;
  flex-direction: column;
  align-items: center;
`;

export const SearchInput = styled.input`
  width: var(--common-medium-size);
  max-width: 90%;
  margin: 10px 0;
  padding: 10px;
  border: 1px solid #ccc;
  border-radius: 5px;
`;

export const SearchError = styled.div<{ hidden: boolean }>`
  width: var(--common-medium-size);
  max-width: 90%;
  color: var(--common-color-error);
  height: 20px;

  ${props => props.hidden && css`
    visibility: hidden;
  `}
`;