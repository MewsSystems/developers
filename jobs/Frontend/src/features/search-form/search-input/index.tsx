import styled from "styled-components";
import { Props } from "./model";

export const SearchInput = (props: Props) => {
    return (
        <StyledSearch
            type="text"
            placeholder="Search for a movie..."
            value={props.keyword}
            onChange={(e) => props.onChange(e.target.value)}
        />
    );
}

const StyledSearch = styled.input`
    border: 1px solid #ccc;
    border-radius: 5px;
    padding: 5px;
    font-size: 1.2em;
    width: 100%;
    box-sizing: border-box;
    margin-bottom: 10px;
`;
