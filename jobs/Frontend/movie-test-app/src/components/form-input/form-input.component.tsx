import { InputHTMLAttributes, FC } from 'react';

import { StyledLabel, StyledInput, Group } from './form-input.styles.tsx';

type FormInputProps = {
  label: string;
  value: string;
  displayfullsearch: string;
} & InputHTMLAttributes<HTMLInputElement>;

const FormInput: FC<FormInputProps> = ({ label, value, displayfullsearch, ...otherProps }) => {
  return (
    <Group>
      <StyledInput {...otherProps} value={value} displayfullsearch={displayfullsearch} />
      {label && <StyledLabel shrink={value.toString()}>{label}</StyledLabel>}
    </Group>
  );
};

export default FormInput;
