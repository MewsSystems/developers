import styled from "styled-components";

export const SearchContainer = styled.div`
  display: flex;
  justify-content: center;
  align-items: center;
  margin-bottom: 20px;
`;

export const StyledInput = styled.input`
  width: 50%; // Responsive width
  padding: 10px 15px;
  font-size: 16px;
  border: 2px solid #ccc;
  border-radius: 8px;
  transition: border-color 0.3s ease-in-out, box-shadow 0.3s ease-in-out;

  &:hover,
  &:focus {
    border-color: #007bff;
    box-shadow: 0 2px 8px rgba(0, 123, 255, 0.2);
    outline: none;
  }
`;
