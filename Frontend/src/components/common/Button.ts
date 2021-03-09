import styled from 'styled-components';

const Button = styled.button<ButtonProps>`
  display: inline-block;
  vertical-align: middle;
  padding: ${({ theme }) => `${theme.space[3]} ${theme.space[4]}`};
  font-size: ${({ theme }) => theme.fontSizes.m};
  cursor: pointer;

  color: ${({ variant, theme }) =>
    variant === 'primary' ? theme.colors.background : theme.colors.primary};

  background-color: ${({ variant, theme }) =>
    variant === 'primary' ? theme.colors.primary : theme.colors.background};

  border-color: ${({ theme }) => theme.colors.primaryDark};

  &:hover:not(:disabled),
  &:active:not(:disabled),
  &:focus {
    outline: 0;
    color: ${({ theme }) => theme.colors.background};
    border-color: ${({ theme }) => theme.colors.primaryLight};
    background-color: ${({ theme }) => theme.colors.primaryLight};
    cursor: pointer;
  }

  &:disabled {
    opacity: 0.6;
    filter: saturate(60%);
    cursor: not-allowed;
    pointer-events: all !important;
  }
`;

Button.defaultProps = {
  variant: 'primary',
};

export interface ButtonProps extends React.HTMLProps<HTMLButtonElement> {
  variant?: 'primary' | 'secondary';
}

export default Button;
