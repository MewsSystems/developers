// components/ThemeToggle/ThemeToggle.tsx
import React from 'react';
import styled from 'styled-components';
import { useTheme } from '../../context/ThemeContext';

const ToggleButton = styled.button.attrs({
  className: 'theme-toggle-button',
})`
  padding: ${({ theme }) => theme.spacing.sm} ${({ theme }) => theme.spacing.md};
  background-color: transparent;
  color: ${({ theme }) => theme.colors.button.inverseText};
  border: 2px solid ${({ theme }) => theme.colors.button.background};
  border-radius: ${({ theme }) => theme.borderRadius.md};
  cursor: pointer;
  transition:
    border-color 0.2s,
    color 0.2s;

  &:hover {
    border-color: ${({ theme }) => theme.colors.button.hover};
    color: ${({ theme }) => theme.colors.button.hover};
  }
`;

export const ThemeToggle: React.FC = () => {
  const { toggleTheme } = useTheme();

  return <ToggleButton onClick={toggleTheme}>Toggle Theme</ToggleButton>;
};
