import StyledButton from './button.styles';
import { FC } from 'react';

interface ButtonProps {
  children: React.ReactNode;
  onClick: () => void;
}

const Button: FC<ButtonProps> = ({ children, ...props }) => {
  return <StyledButton {...props}>{children}</StyledButton>;
};

export default Button;
