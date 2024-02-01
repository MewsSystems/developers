import React, { FC } from 'react';
import StyledInput from './Input.styles';

const Input: FC<React.InputHTMLAttributes<HTMLInputElement>> = (props) => {
    return <StyledInput {...props} />;
};

export default Input;
