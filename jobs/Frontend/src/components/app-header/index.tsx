import styled from "styled-components";

export const AppHeader = () => {
    return (
        <>
            <StyledHeader>TMSE: The Movie Search Engine</StyledHeader>
            <hr />
        </>
    );
}

export const StyledHeader = styled.div`
    font-family: Luminari;
    font-size: 50px;
`;