import styled from 'styled-components';
import Box from '@/components/Box';
import { PaginationButtonProps } from '@/components/Pagination/Pagination';

export const PaginationButton = styled(Box)
  .attrs((attrs) => ({...attrs, as: 'button'}))
  .withConfig({shouldForwardProp: (prop) => prop !== 'active'})<PaginationButtonProps>`
  appearance: none;
  border: 1px solid #d7d7d7;
  cursor: pointer; 
  background-color: white;
  font-weight: ${({active}) => active ? 'bold' : undefined};
  
  &[disabled]{
    cursor: no-drop;
  }
`;
