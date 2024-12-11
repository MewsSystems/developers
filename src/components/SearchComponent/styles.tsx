import styled from 'styled-components';
import { theme } from '../../styles/theme';

export const Wrapper = styled.div`
  padding: 16px;
  border: 1px solid ${theme.colors.black[300]};
  border-radius: 4px;
  background-color: ${theme.colors.white[300]};
  display: flex;
  flex-direction: column;
`;

export const TextInput = styled.input.attrs({ type: 'text' })`
  padding: 20px;
  font-size: 24px;
  border: 2px solid #ccc;
  border-radius: 10px;
  width: 100%;
  max-width: 400px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  outline: none;

  ::placeholder {
    color: #aaa;
  }

  :focus {
    border-color: ${theme.colors.red[300]};
  }
`;

export const OptionLabel = styled.div`
  display: flex;
  justify-content: flex-start;
  width: 100%;
`;

export const OptionsSection = styled.fieldset`
  border-radius: 4px;
  margin-top: ${theme.spacing.sm}px;
  display: flex;
  align-items: center;
  @media screen and (max-width: 480px) {
    flex-direction: column;
    line-height: 2rem;
  }
`;

export const Radio = styled.input.attrs({ type: 'radio' })``;

export const RadioLabel = styled.label`
  display: flex;
  justify-content: flex-start;
  width: 100%;
`;
