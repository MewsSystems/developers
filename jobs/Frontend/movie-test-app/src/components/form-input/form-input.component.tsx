import { InputHTMLAttributes, FC } from 'react';

import { StyledLabel, StyledInput, Group } from './form-input.styles.tsx';

type FormInputProps = {
  label: string;
  value: string;
  displayFullSearch: boolean;
} & InputHTMLAttributes<HTMLInputElement>;

const FormInput: FC<FormInputProps> = ({ label, value, displayFullSearch, ...otherProps }) => {
  return (
    <Group>
      <StyledInput {...otherProps} value={value} displayFullSearch={displayFullSearch} />
      {label && <StyledLabel shrink={value.toString()}>{label}</StyledLabel>}
    </Group>
  );
};

export default FormInput;
