import React, { ChangeEvent, FC } from 'react';

export const Checkbox: FC<{
  label: string;
  value: string;
  isChecked: boolean;
  onChange: (e: ChangeEvent<HTMLInputElement>) => void;
}> = ({ label, value, isChecked, onChange }) => {
  return (
    <label>
      <input type="checkbox" value={value} checked={isChecked} onChange={onChange} />
      {label}
    </label>
  );
};
