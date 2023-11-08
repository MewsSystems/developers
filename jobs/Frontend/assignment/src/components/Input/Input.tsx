import React, { useState } from "react";
import styled from "styled-components";
import CloseIcon from "@material-ui/icons/HighlightOff";
import SearchIcon from "@material-ui/icons/Search";

export interface InputProps {
  label: string;
  value: string;
  onChange: (value: string) => void;
}

const InputWrapper = styled.div`
  display: flex;
  align-items: center;

  width: 100%;
  min-height: 56px;

  padding: 16px 8px;

  border-radius: 4px;
  border: 2px solid ${({ theme }) => theme.colors.primary.main};

  transition: border-color 0.2s ease-in-out;
`;

const FieldWrapper = styled.div`
  width: 100%;
  height: 100%;

  position: relative;
`;

const InputLabel = styled.label<{ focused: boolean }>`
  position: absolute;
  top: ${({ focused }) => (focused ? "-100%" : "50%")};
  left: ${({ focused }) => (focused ? "-30px" : "0")};

  font-size: ${({ focused, theme }) =>
    focused ? theme.fonts.labelMedium.fontSize : theme.fonts.labelLarge.fontSize};

  transform: ${({ focused }) => (focused ? "translateY(0)" : "translateY(-50%)")};
  transition: all 0.2s ease-in-out;

  pointer-events: none;
`;

const InputField = styled.input`
  width: 100%;
  height: 100%;

  border: none;
  outline: none;

  font-size: ${({ theme }) => theme.fonts.bodyLarge};
`;

const IconContainer = styled.div`
  width: 24px;
  height: 24px;

  margin-right: 8px;

  display: flex;
  align-items: center;
  justify-content: center;
`;

const ClearButton = styled.button`
  width: 24px;
  height: 24px;

  margin-left: 8px;
  border: none;
  outline: none;

  background-color: transparent;
  border-radius: 50%;

  transition: all 0.2s ease-in-out;
  &:hover,
  &:focus {
    transform: scale(1.1);
  }
`;

export function Input({ label, value, onChange }: InputProps) {
  const [focused, setFocused] = useState(false);

  const handleInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    onChange(event.target.value);
  };

  const handleClearClick = () => {
    onChange("");
  };

  return (
    <InputWrapper>
      <IconContainer>
        <SearchIcon />
      </IconContainer>
      <FieldWrapper>
        <InputLabel focused={focused || !!value} htmlFor="input-field">
          {label}
        </InputLabel>
        <InputField
          id="input-field"
          type="text"
          value={value}
          onChange={handleInputChange}
          onFocus={() => setFocused(true)}
          onBlur={() => setFocused(false)}
        />
      </FieldWrapper>
      {!!value && (
        <ClearButton onClick={handleClearClick}>
          <CloseIcon />
        </ClearButton>
      )}
    </InputWrapper>
  );
}