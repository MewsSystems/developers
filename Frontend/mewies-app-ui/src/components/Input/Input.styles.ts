import styled from 'styled-components'
import { Colors } from '../../utils/constants/color.constants'
import { setMargin } from '../../utils/helpers/component.helpers'
import { Font } from '../../utils/constants/font.constants'

export interface InputStyleProps {
    error?: string
    disabled?: boolean
}

export interface InputWrapperStyleProps {
    margin?: Array<0 | 4 | 8 | 16 | 32>
    disabled?: boolean
    fullWidth?: boolean
}

export const StyledInput = styled.input<InputStyleProps>`
    font-family: Helvetica, sans-serif;
    color: ${props => (props.disabled ? Colors.gray : Colors.black)};
    width: 100%;
    padding: 12px;
    display: inline-block;
    border-radius: 4px;
    border: solid 1px ${props => (!props.error ? Colors.gray : Colors.black)};
    outline: transparent;
    font-size: 16px;
    font-weight: 400;
    transition-duration: 0.5s;
    &:focus {
        border: solid 1px
            ${props => (!props.error ? Colors.yellow : Colors.alert)};
    }
    ::placeholder {
        color: ${Colors.gray};
    }
`

export const StyledInputWrapper = styled.div<InputWrapperStyleProps>`
    width: ${props => (props.fullWidth ? '100%' : 'auto')};
    min-width: 120px;
    opacity: ${props => (props.disabled ? '0.5' : '1')};
    ${props => props.margin && setMargin(props.margin)};
`

export const InputLabel = styled.label`
    font-family: ${Font.Montserrat};
    color: ${Colors.black};
    font-weight: 700;
    margin-bottom: 4px;
    display: block;
    font-size: 12px;
`

export const Error = styled.span`
    display: block;
    font-size: 12px;
    font-weight: 300;
    margin-top: 4px;
    line-spacing: 14px;
    color: ${Colors.alert};
`

export const InputAutosuggestWrapper = styled.div`
    position: relative;
`

export const SuggestionList = styled.ul`
    width: 100%;
    list-style: none;
    position: absolute;
    background: ${Colors.white};
    z-index: 5;
    border-radius: 5px;
    border: 1px solid ${Colors.gray};
    -webkit-box-shadow: 0px 4px 5px 0px rgba(7, 64, 58, 0.05);
    -moz-box-shadow: 0px 4px 5px 0px rgba(7, 64, 58, 0.05);
    box-shadow: 0px 4px 5px 0px rgba(7, 64, 58, 0.05);
    max-height: 360px;
    overflow: scroll;
`

export const StyledSuggestion = styled.li<{ isSelected?: boolean }>`
    display: flex;
    align-items: center;
    padding: 12px;
    cursor: pointer;
    border-bottom: 1px solid ${Colors.gray};
    p {
        color: ${props => (props.isSelected ? Colors.yellow : Colors.black)};
        margin-left: 10px;
        &:hover {
            color: ${Colors.yellow};
        }
    }
    &:focused {
        border: 1px solid ${Colors.yellow};
        color: ${Colors.yellow};
    }
`
