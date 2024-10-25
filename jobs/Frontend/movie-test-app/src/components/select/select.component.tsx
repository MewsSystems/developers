import { StyledSelect, Group, StyledLabel } from './select.styles.tsx';
import { FC } from 'react';

interface SelectProps {
  onChange: (e: React.ChangeEvent<HTMLSelectElement>) => void;
  options: string[];
  value?: string;
  label?: string;
}

const Select: FC<SelectProps> = ({ onChange, options, label, ...otherProps }) => {
  return (
    <Group>
      <StyledSelect onChange={onChange} {...otherProps}>
        {options.map((option: string) => (
          <option key={option} value={option}>
            {option}
          </option>
        ))}
      </StyledSelect>
      {label && <StyledLabel>{label}</StyledLabel>}
    </Group>
  );
};

export default Select;
