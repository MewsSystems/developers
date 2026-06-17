import React, { FC } from 'react';
import StyledButton from './Button.styles';

export interface ButtonProps
    extends React.ButtonHTMLAttributes<HTMLButtonElement> {
    variant?: 'primary' | 'secondary' | 'icon';
    size?: 'normal' | 'small';
}

const Button: FC<ButtonProps> = (props) => {
    return <StyledButton {...props}>{props.children}</StyledButton>;
};

export default Button;
