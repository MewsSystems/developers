import styled from "styled-components"
import { fontSizes } from "../../../tokens/fontSizes"

export const SearchBarWrapper = styled.div`
    display: flex;
    align-items: center;
    justify-content: center;
    gap: 15px;
    margin: 30px 0;
`
export const Label = styled.label`
    font-size: ${fontSizes.xs};
    font-weight: 500;
    white-space: nowrap;
`

export const Input = styled.input`
    width: 100%;
    max-width: 400px;
    padding: 12px;
    font-size: ${fontSizes.xs};
    border: none;
    border-radius: 4px;

    &:focus {
        outline: none;
    }

    &:focus-visible {
        outline: 2px solid white;
        outline-offset: 4px;
    }
`