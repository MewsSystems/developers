import styled from "styled-components";

export const NavigationHeader = styled.header`
  background-color: #34495e;
  padding: 20px;
  color: white;
  display: flex;
  justify-content: flex-start;
  align-items: center;
`;

export const BackButton = styled.button`
  background-color: #2980b9;
  color: white;
  border: none;
  padding: 10px 20px;
  cursor: pointer;
  font-size: 16px;
  border-radius: 5px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  transition: background-color 0.3s, box-shadow 0.3s;

  &:hover {
    background-color: #3498db;
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
  }
`;
