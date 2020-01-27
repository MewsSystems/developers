import React from 'react'
import {
    Error,
    InputWrapperStyleProps,
    StyledInputWrapper,
} from '../Input.styles'

export interface InputWrapperProps extends InputWrapperStyleProps {
    helperText?: string
    error?: string
}

export const InputWrapper: React.FC<InputWrapperProps> = props => (
    <StyledInputWrapper margin={props.margin} disabled={props.disabled}>
        {props.children}
        {props.error && <Error>{props.error}</Error>}
    </StyledInputWrapper>
)
