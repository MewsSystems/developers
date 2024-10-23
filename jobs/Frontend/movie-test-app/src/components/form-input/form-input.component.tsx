import { InputHTMLAttributes, FC } from 'react';

import { FormInputLabel, Input, Group } from './form-input.styles';

type FormInputProps = { label: string; searchQuery: string } & InputHTMLAttributes<HTMLInputElement>;

const FormInput: FC<FormInputProps> = ({ label, searchQuery, ...otherProps }) => {
  return (
    <Group>
      <Input {...otherProps} />
      {label && <FormInputLabel searchquery={searchQuery}>{label}</FormInputLabel>}
    </Group>
  );
};

export default FormInput;
