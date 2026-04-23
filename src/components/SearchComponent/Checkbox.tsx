import React, { ChangeEvent, FC } from 'react';
import { OptionLabel } from './styles';

export const Checkbox: FC<{
  label: string;
  value: string;
  isChecked: boolean;
  onChange: (e: ChangeEvent<HTMLInputElement>) => void;
}> = ({ label, value, isChecked, onChange }) => {
  return (
    <OptionLabel>
      <input type="checkbox" value={value} checked={isChecked} onChange={onChange} />
      {label}
    </OptionLabel>
  );
};
