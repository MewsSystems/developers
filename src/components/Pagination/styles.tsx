import { styled } from 'styled-components';
import { theme } from '../../styles/theme';

export const PaginationWrapper = styled.div`
  padding: ${theme.spacing.md}px;
  display: flex;
  justify-content: space-around;
`;

export const PageButton = styled.button`
  flex: 1;
  outline: none;
  border: 1px solid black;
  display: flex;
  justify-content: center;
  align-items: center;
  padding: ${theme.spacing.sm}px;
  background-color: ${theme.colors.yellow[200]};
  border-radius: 4px;
  cursor: pointer;
  color: white;
`;
