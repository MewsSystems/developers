import styled from 'styled-components';

export interface ButtonProps extends React.HTMLProps<HTMLButtonElement> {
  variant?: 'primary' | 'secondary';
}

const Button = styled.button<ButtonProps>`
  display: inline-block;
  vertical-align: middle;
  padding: ${(props) => `${props.theme.space[3]} ${props.theme.space[4]}`};
  font-size: ${(props) => props.theme.fontSizes.m};
  cursor: pointer;

  color: ${(props) =>
    props.variant === 'primary'
      ? props.theme.colors.background
      : props.theme.colors.primary};

  background-color: ${(props) =>
    props.variant === 'primary'
      ? props.theme.colors.primary
      : props.theme.colors.background};

  border-color: ${(props) => props.theme.colors.primaryDark};

  &:hover:not(:disabled),
  &:active:not(:disabled),
  &:focus {
    outline: 0;
    color: ${(props) => props.theme.colors.background};
    border-color: ${(props) => props.theme.colors.primaryLight};
    background-color: ${(props) => props.theme.colors.primaryLight};
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

export default Button;
