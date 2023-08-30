import styled from "styled-components"
import { Props } from "./model"
import { Conditional } from "components/conditional";
import { StyledButton } from "components/styled-button";

export const PaginationControl = (props: Props) => {
    return (
        <Conditional showIf={props.totalPages > 1}>
            <Spaced>
                <StyledButton onClick={() => props.onChange(1)} disabled={props.page === 1}>First</StyledButton>
                <StyledButton onClick={() => props.onChange(props.page - 1)} disabled={props.page === 1}>Previous</StyledButton>
                <span>{props.page} / {props.totalPages}</span>
                <StyledButton onClick={() => props.onChange(props.page + 1)} disabled={props.page === props.totalPages}>Next</StyledButton>
                <StyledButton onClick={() => props.onChange(props.totalPages)} disabled={props.page === props.totalPages}>Last</StyledButton>
            </Spaced>
        </Conditional>
    )
}

const Spaced = styled.div`
    margin-top: 10px;
    & > * {
        margin-right: 5px;
    }
`;