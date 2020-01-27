import React, { RefObject, useMemo, forwardRef, KeyboardEvent } from 'react'
import { InputWrapper, InputWrapperProps } from '../InputWrapper/InputWrapper'
import { InputLabel, InputStyleProps, StyledInput } from '../Input.styles'

import { generateID } from '../../../utils/helpers/component.helpers'

export interface InputProps
    extends Partial<HTMLInputElement>,
        InputStyleProps,
        InputWrapperProps {
    onChange(event: React.ChangeEvent<HTMLInputElement>): void
    onKeyDown?(event: KeyboardEvent<any>): void
    onBlur?(e: any): void
    onFocus?(): void
    label?: string
    ref?: RefObject<HTMLInputElement> | null
}

export const InputText: React.FC<InputProps> = forwardRef((props, ref) => {
    const id = useMemo(() => {
        return props.id ? props.id : generateID(6)
    }, [props.id])

    return (
        <InputWrapper
            error={props.error}
            helperText={props.helperText}
            margin={props.margin}
            fullWidth={props.fullWidth}
            disabled={props.disabled}
        >
            {props.label && <InputLabel htmlFor={id}>{props.label}</InputLabel>}
            <StyledInput
                id={id}
                name={props.name}
                type={props.type}
                disabled={props.disabled}
                onChange={props.onChange}
                onBlur={props.onBlur}
                onKeyDown={props.onKeyDown}
                onFocus={props.onFocus}
                placeholder={props.placeholder}
                required={props.required}
                value={props.value}
                error={props.error}
                ref={ref}
            />
        </InputWrapper>
    )
})
