import React from "react";
import styled from "styled-components";

interface IInputTextProps {
  value: string;
  onChange: (event: React.ChangeEvent<HTMLInputElement>) => void;
  placeholder?: string;
}

export const InputText: React.FC<IInputTextProps> = (props) => {
  const { onChange, value, placeholder } = props;

  return (
    <StyledInputContainer
      type={"text"}
      value={value}
      placeholder={placeholder}
      onChange={onChange}
    />
  );
};

const StyledInputContainer = styled.input`
  &:focus {
    border: ${({ theme }) => `1px solid ${theme.color.darkBlue}`};
    outline: none;
    box-shadow: rgba(0, 0, 0, 0.24) 0 3px 8px;
  }

  grid-area: input;
  position: relative;
  height: 3rem;
  width: 100%;
  font-size: 1.2rem;
  border-radius: 8px;
  background-color: ${({ theme }) => theme.color.white};
  border: ${({ theme }) => `1px solid ${theme.color.blue}`};
  padding: 0.5rem;
  transition: box-shadow 200ms ease-in-out, border 200ms ease-in-out;
`;
