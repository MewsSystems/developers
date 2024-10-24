import StyledSelect from './select.styles.tsx';
import { FC } from 'react';

interface SelectProps {
  onChange: (e: React.ChangeEvent<HTMLSelectElement>) => void;
  options: string[];
}

const Select: FC<SelectProps> = ({ onChange, options }) => {
  return (
    <StyledSelect onChange={onChange}>
      {options.map((option: string) => (
        <option key={option} value={option}>
          {option}
        </option>
      ))}
    </StyledSelect>
  );
};

export default Select;
