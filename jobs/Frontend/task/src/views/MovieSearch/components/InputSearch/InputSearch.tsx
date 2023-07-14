import { ChangeEvent, FC, KeyboardEvent, useEffect, useState } from "react";
import Loader from "src/components/Loader";
import { KeyboardSpecialKey } from "src/enums/KeyboardSpecialKey";
import useDebounce from "src/helpers/useDebounce";
import { InputSearchProps } from "src/views/MovieSearch/components/InputSearch/InputSearchProps";
import { ReactComponent as SearchIcon } from "src/assets/search.svg";
import styled from "styled-components";

/**
 * Input that changes value after debounce time
 *
 * @onDebounce* - function that returns a value after debounce time
 * @onEnter - function that returns a value after pressing enter
 *          - cancels debounce if used
 */
export const InputSearch: FC<InputSearchProps> = (props) => {
  const {
    onDebounce,
    onChange,
    onEnter,
    debounceTime,
    innerRef,
    placeholder,
    loading,
  } = props;

  const [value, setValue] = useState<string>("");
  const [pressedEnter, setPressedEnter] = useState<boolean>(false);
  const debouncedValue = useDebounce<string>(value, debounceTime || 1000);

  useEffect(() => {
    if (debouncedValue !== undefined && !pressedEnter) {
      onDebounce(value);
    }

    if (pressedEnter) setPressedEnter(false);
  }, [debouncedValue]);

  const handleEnterPress = (e: KeyboardEvent<HTMLInputElement>) => {
    if (e.key === KeyboardSpecialKey.Enter) {
      e.stopPropagation();
      onEnter?.(e.currentTarget.value);
      setPressedEnter(true);
    }
  };

  const handleOnChange = (e: ChangeEvent<HTMLInputElement>) => {
    onChange?.(e.currentTarget.value);
    setValue(e.currentTarget.value);
  };

  return (
    <Wrap>
      <Input
        ref={innerRef}
        value={value}
        placeholder={placeholder}
        onChange={handleOnChange}
        onKeyDown={handleEnterPress}
      />
      <Loader loading={!!loading} />
    </Wrap>
  );
};

const Input = styled.input<InputSearchProps>`
  padding: 12px;
  font-size: 16px;
  border: 1px solid gray;
  border-radius: 4px;
  min-width: 200px;

  &::placeholder {
    color: #a3a3a3;
  }
`;

const Wrap = styled.div`
  display: flex;
  align-items: center;
  gap: 8px;
`;
