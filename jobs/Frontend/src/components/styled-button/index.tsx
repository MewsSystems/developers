import styled, { css } from "styled-components";

export const ButtonStyle = css`
    font-size: 16px;
    color: black;
    text-decoration: none;
    background: radial-gradient(white, lightgray);
    padding: 5px 10px;
    border-radius: 5px;
    border: 1px solid gray;
    box-shadow: 10px rgba(255, 255, 255, 0.3);
    &:hover {
        background: #f1f1f1;
        cursor: pointer;
    }
    &:disabled {
        color: gray;
        background: darkgray;
        cursor: not-allowed;
    }
`;

export const StyledButton = styled.button`
    ${ButtonStyle}
`;
