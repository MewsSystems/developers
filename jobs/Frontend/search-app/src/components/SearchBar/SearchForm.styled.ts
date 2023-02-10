import styled from "styled-components";

export const Form = styled.form`
  display: flex;
  align-items: center;
  width: 80%;
  background-color: ${props => props.theme.backgroundColorSecondary};
  color: ${props => props.theme.textColorSecondary};
  height: 40px;
  border-radius: 2px;

  :focus-within {
    background-color: ${props => props.theme.backgroundColorHover};
  }
`;

export const Input = styled.input`
  padding: 10px;
  font-size: 18px;
  border: 0;
  outline: 0;
  background-color: ${props => props.theme.backgroundColorSecondary};
  color: ${props => props.theme.textColorPrimary};
  width: 100%;
`;

export const Icon = styled.i`
  font-size: 18px;
  margin: 0 10px;
`;
