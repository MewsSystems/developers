import { Search, TimesCircle } from '@styled-icons/fa-solid';
import { useRef } from 'react';
import {
  InputContainer,
  Input,
  InputContainerProps,
  InputClearButton,
  InputIconContainer,
} from './styled';

interface SearchInputProps extends InputContainerProps {
  placeholderText?: string;
  value: string;
  onChange: (value: string) => void;
}

const SearchInput = (props: SearchInputProps) => {
  const inputRef = useRef<HTMLInputElement>(null);
  const focusInput = () => inputRef.current && inputRef.current.focus();

  return (
    <InputContainer maxWidth={props.maxWidth}>
      <InputIconContainer>
        <Search size="1.5rem" onClick={focusInput} />
      </InputIconContainer>
      <Input
        ref={inputRef}
        type="text"
        value={props.value}
        onChange={(e) => props.onChange(e.currentTarget.value)}
        placeholder={props.placeholderText}
      />
      {props.value && (
        <InputClearButton type="button" onClick={() => props.onChange('')}>
          <TimesCircle size="1.5rem" />
        </InputClearButton>
      )}
    </InputContainer>
  );
};

export default SearchInput;
