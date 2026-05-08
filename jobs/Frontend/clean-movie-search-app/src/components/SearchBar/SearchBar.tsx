import React from 'react';
import styled from 'styled-components';

const SearchContainer = styled.div`
  margin: ${({ theme }) => `${theme.spacing.lg} 0`};
  display: flex;
  align-items: center;
  gap: ${({ theme }) => theme.spacing.md};
`;

const Input = styled.input`
  width: 100%;
  max-width: 500px;
  padding: ${({ theme }) => `${theme.spacing.md} ${theme.spacing.lg}`};
  font-size: 16px;
  border: 2px solid ${({ theme }) => theme.colors.input.border};
  border-radius: ${({ theme }) => theme.borderRadius.md};
  background-color: ${({ theme }) => theme.colors.input.background};
  color: ${({ theme }) => theme.colors.text.primary};
  transition:
    border-color 0.2s,
    background-color 0.3s,
    color 0.3s;

  &:focus {
    outline: none;
    border-color: ${({ theme }) => theme.colors.input.focusBorder};
  }

  &::placeholder {
    color: ${({ theme }) => theme.colors.text.secondary};
  }
`;

interface SearchBarProps {
  value: string;
  onChange: (value: string) => void;
  placeholder?: string;
}

export const SearchBar: React.FC<SearchBarProps> = ({
  value,
  onChange,
  placeholder = 'Search for what to watch next...',
}) => {
  return (
    <SearchContainer>
      <Input
        type="text"
        value={value}
        onChange={(e) => onChange(e.target.value)}
        placeholder={placeholder}
      />
    </SearchContainer>
  );
};
