import styled from "styled-components";
import { colors } from "../tokens/colors";

export const StyledButton = styled.button`
    display: block;
    background-color: ${colors.btnPrimary};
    color: white;
    border: none;
    padding: 12px 24px;
    font-size: 16px;
    font-weight: 600;
    border-radius: 4px;
    transition: transform 0.2s ease, background-color 0.2s ease;

    &:hover {
        transform: scale(1.03);
        background-color: ${colors.btnSecondary};
    }

    &:focus-visible {
        outline: 2px solid white; /* Add visible focus for accessibility */
        outline-offset: 4px;
    }
`