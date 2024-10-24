import { InputHTMLAttributes, FC } from 'react';

import { StyledLabel, StyledInput, Group } from './form-input.styles.tsx';

type FormInputProps = { label: string; value: string } & InputHTMLAttributes<HTMLInputElement>;

const FormInput: FC<FormInputProps> = ({ label, value, ...otherProps }) => {
  return (
    <Group>
      <StyledInput {...otherProps} value={value} />
      {label && <StyledLabel shrink={value.toString()}>{label}</StyledLabel>}
    </Group>
  );
};

export default FormInput;
