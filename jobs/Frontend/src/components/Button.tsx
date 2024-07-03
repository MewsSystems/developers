import styled from "styled-components";

const Button = styled.button`
    padding: 8px 16px;
    background-color: ${props => props.theme.colors.primary};
    color: white;
    border: none;
    border-radius: 4px;
    cursor: pointer;
`;

export default Button;
