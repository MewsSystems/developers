import React from 'react'
import { Default } from './Button.styles'

interface ButtonProps {
    onClick?: (event: React.MouseEvent<Element>) => void
    disabled?: boolean
    type?: 'submit' | 'reset' | 'button'
}

export const Button: React.FC<ButtonProps> = props => {
    return <Default {...props}>{props.children}</Default>
}
