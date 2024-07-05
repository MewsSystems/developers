import styled from "styled-components";

export const PaginationContainer = styled.div`
  background-color: #34495e;
  color: white;
  padding: 20px 0;
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 10px;
`;

export const Button = styled.button`
  padding: 10px 20px;
  font-size: 16px;
  color: white;
  background-color: #2980b9; 
  border: none;
  border-radius: 5px;
  cursor: pointer;
  transition: background-color 0.3s;

  &:hover {
    background-color: #3498db; 

  &:disabled {
    background-color: #7f8c8d;
    cursor: default;
  }
`;

export const PageIndicator = styled.span`
  font-size: 16px;
`;
