import styled from 'styled-components'
import { Colors } from '../../utils/constants/color.constants'

export const Default = styled.button<{ disabled?: boolean }>`
    display: block;
    background: ${Colors.black};
    color: ${Colors.white};
    font-size: 13px;
    font-weight: 700;
    border: none;
    min-width: 120px;
    padding: 12px;
    transition-duration: 0.8s;
    ${props => props.disabled && 'opacity: 0.3;'}
    &:hover {
        opacity: ${props => (props.disabled ? '0.3' : '0.7')};
    ]
`
