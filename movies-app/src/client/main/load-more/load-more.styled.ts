import styled from 'styled-components';

export const LoadMoreWrapper = styled.div`
  display: flex;
  justify-content: center;
`;

export const LoadMoreButton = styled.div`
  width: 90%;
  height: 40px;
  border: 1px solid var(--common-color-blue-1);
  background-color: #ffffff;
  display: flex;
  justify-content: center;
  align-items: center;
  text-align: center;
  cursor: pointer;

  &:hover {
    background-color: var(--common-color-blue-1);
  }
`;