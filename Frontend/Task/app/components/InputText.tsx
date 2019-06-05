import React from "react";
import { InputWrapper, InputStyled } from "./styled";
type InputProps = {
  onChange: (e: React.FormEvent<HTMLInputElement>) => void;
  placeholder?: string;
};
export const InputText = ({ onChange, placeholder }: InputProps) => (
  <InputWrapper>
    <InputStyled onChange={onChange} placeholder={placeholder} />
  </InputWrapper>
);
