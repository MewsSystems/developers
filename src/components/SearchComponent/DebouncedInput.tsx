import React, { useState, useMemo, useRef, useEffect, FC } from 'react';
import { debounce } from 'lodash';
import { TextInput } from './styles';
import { connect, useDispatch } from 'react-redux';
import { Dispatch } from 'redux';

const useDebounce = (callback?: () => void) => {
  const ref = useRef<() => void | undefined>();

  useEffect(() => {
    ref.current = callback;
  }, [callback]);

  const debouncedCallback = useMemo(() => {
    const func = () => {
      ref.current?.();
    };

    return debounce(func, 1000);
  }, []);

  return debouncedCallback;
};

export const DebouncedInput: FC<{
  handleOnChange: (query: string) => void;
  placeholder?: string;
  defaultValue?: string;
}> = ({ handleOnChange, placeholder, defaultValue }) => {
  const [value, setValue] = useState(defaultValue || '');

  const onChange = () => {
    handleOnChange(value);
  };

  const debouncedOnChange = useDebounce(onChange);

  return (
    <TextInput
      onChange={(e) => {
        debouncedOnChange();
        setValue(e.target.value);
      }}
      placeholder={placeholder ?? 'Search...'}
      defaultValue={defaultValue}
    />
  );
};
