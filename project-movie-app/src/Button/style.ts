import styled from "styled-components";

export const StyledButton = styled.button`
    display: block;
    background-color: #e50914;
    color: white;
    border: none;
    padding: 12px 24px;
    font-size: 16px;
    font-weight: 600;
    border-radius: 4px;
    cursor: pointer;
    margin: 0 auto;
    transition: transform 0.2s ease, background-color 0.2s ease;

    &:hover {
        transform: scale(1.03);
        background-color: #f6121d;
    }

    &:focus {
        outline: 2px solid white; /* Add visible focus for accessibility */
        outline-offset: 4px;
    }
`