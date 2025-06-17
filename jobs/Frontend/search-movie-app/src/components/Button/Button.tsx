import { StyledButton } from './Button.styles';
import type { ButtonHTMLAttributes } from 'react';

export const Button = ({ ...props }: ButtonHTMLAttributes<HTMLButtonElement>) => {
  return <StyledButton {...props}>{props.children}</StyledButton>;
};
