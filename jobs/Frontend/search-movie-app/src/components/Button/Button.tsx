import type { ButtonHTMLAttributes } from 'react';
import { StyledButton } from './Button.styles';

export const Button = ({ ...props }: ButtonHTMLAttributes<HTMLButtonElement>) => {
  return <StyledButton {...props}>{props.children}</StyledButton>;
};
