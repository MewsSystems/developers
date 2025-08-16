import styled from "styled-components";

export const PaginationWrapper = styled.div`
  display: flex;
  flex-direction: row;
`;

export const TableButton = styled.button`
  font-size: 24px;
  background-color: cornflowerblue;
  padding: 8px;
  width: 80px;
  border-radius: 8px;
  &:disabled {
    background-color: dimgrey;
    cursor: no-drop;
  }
  &:hover:enabled {
    cursor: pointer;
    background-color: dodgerblue;
  }
`;

export const PaginationNumbering = styled.span`
  align-self: center;
  font-size: 16px;
  font-weight: normal;
`;

export const THead = styled.thead`
  margin-bottom: 8px;
  font-weight: bold;
`;

export const Tr = styled.tr`
  border-bottom: 1px solid whitesmoke;
  font-size: 16px;
`;

export const Table = styled.table`
  margin-bottom: 16px;
  width: 100%;
  border-collapse: collapse;
`;

export const Td = styled.td<{ $textAlign?: string; $width?: string}>`
  padding-bottom: 8px;
  padding-top: 8px;
  text-align: ${props => props.$textAlign};
  width: ${props => props.$width};
  white-space: nowrap;
  text-overflow: ellipsis;
`;
