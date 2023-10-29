import styled from 'styled-components';
import { theme } from '../../styles/theme';

export const Wrapper = styled.div`
  padding: 16px;
  border: 1px solid ${theme.colors.black[300]};
  border-radius: 4px;
  background-color: ${theme.colors.white[300]};
`;

export const TextInput = styled.input.attrs({ type: 'text' })`
  padding: 20px; /* Adjust the padding to make it "huge" */
  font-size: 24px; /* Adjust the font size */
  border: 2px solid #ccc;
  border-radius: 10px;
  width: 100%;
  max-width: 400px; /* Adjust the maximum width as needed */
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  outline: none;

  ::placeholder {
    color: #aaa;
  }

  :focus {
    border-color: #007bff; /* Change color on focus */
  }
`;
