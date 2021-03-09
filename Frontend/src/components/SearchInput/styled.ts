import styled from 'styled-components';
import Button from '../common/Button';

export const Input = styled.input`
  text-align: center;
  border: none;
  outline: none;
  width: inherit;
  background-color: inherit;
  font-size: inherit;
`;

export interface InputContainerProps {
  maxWidth?: string;
}

export const InputContainer = styled.div<InputContainerProps>`
  max-width: ${({ maxWidth }) => maxWidth || 'none'};
  width: 100%;
  position: relative;
  border: ${({ theme }) => theme.borders.thin};
  border-color: ${({ theme }) => theme.colors.secondaryDark};
  display: flex;
  align-items: center;
  background-color: ${({ theme }) => theme.colors.background};
  color: ${({ theme }) => theme.colors.text};

  :focus-within {
    border-color: ${({ theme }) => theme.colors.primaryLight};
  }
`;

export const InputIconContainer = styled.div`
  display: flex;
  align-items: center;
  padding: ${({ theme }) => theme.space[3]};
`;

export const InputClearButton = styled(Button).attrs({
  variant: 'secondary',
})`
  border: none;
  background-color: inherit !important;
  color: ${({ theme }) => theme.colors.secondaryDark};
  padding: ${({ theme }) => theme.space[3]};

  &:hover:not(:disabled),
  &:active:not(:disabled),
  &:focus {
    color: inherit;
  }
`;
