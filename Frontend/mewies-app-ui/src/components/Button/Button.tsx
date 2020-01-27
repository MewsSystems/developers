import React from 'react'
import { Default } from './Button.styles'

interface ButtonProps {
    onClick?: (event: React.MouseEvent<HTMLButtonElement>) => void
    disabled?: boolean
    type?: 'submit' | 'reset' | 'button'
}

export const Button: React.FC<ButtonProps> = props => {
    return <Default {...props}>{props.children}</Default>
}
