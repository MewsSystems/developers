import styled from "styled-components";
import { Props } from "./model";

export const ErrorMessage = (props: Props) => (
    <>
        <StyledHeader>{props.header}</StyledHeader>
        <StyledText>{props.message}</StyledText>
    </>
)

const StyledHeader = styled.div`
    font-size: 30px;
    font-weight: bold;
    color: #310271;
`;

const StyledText = styled.p`
    font-size: 24px;
`;