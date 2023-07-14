import {
  ChangeEvent,
  FC,
  KeyboardEvent,
  useEffect,
  useMemo,
  useState,
} from "react";
import { InputEnterProps } from "src/components/InputEnter/InputEnterProps";
import { KeyboardSpecialKey } from "src/enums/KeyboardSpecialKey";
import { InputSearchProps } from "src/components/InputSearch/InputSearchProps";
import styled from "styled-components";

/**
 * Input that fires a function onEnter
 */
export const InputEnter: FC<InputEnterProps> = (props) => {
  const { onChange, onEnter, innerRef, placeholder, inputType } = props;

  const [trackedValue, setTrackedValue] = useState(props.value);

  useEffect(() => setTrackedValue(props.value), [props.value]);

  const handleEnterPress = (e: KeyboardEvent<HTMLInputElement>) => {
    if (e.key === KeyboardSpecialKey.Enter) {
      e.stopPropagation();
      onEnter?.(trackedValue);
    }
  };

  const handleOnChange = (e: ChangeEvent<HTMLInputElement>) => {
    onChange?.(e.currentTarget.value);
    setTrackedValue(e.currentTarget.value);
  };

  return (
    <Input
      ref={innerRef}
      value={trackedValue}
      placeholder={placeholder}
      onChange={handleOnChange}
      onKeyDown={handleEnterPress}
      type={inputType}
    />
  );
};

const Input = styled.input<InputSearchProps>`
  padding: 6px;
  font-size: 16px;
  border: 1px solid gray;
  border-radius: 4px;
  width: ${({ value }) => (value ? `${value.length || 0}ch` : "1ch")};
  min-width: 20px;

  &::placeholder {
    color: #a3a3a3;
  }

  /* Hides controls */
  &::-webkit-outer-spin-button,
  &::-webkit-inner-spin-button {
    -webkit-appearance: none;
    margin: 0;
  }
  &::[type="number"] {
    -moz-appearance: textfield;
  }
`;
