import styled from "styled-components";

export const Main = styled.main`
    min-height: calc(100vh - (58.5px));
    padding: 111.1px 30px 60px 30px;

    @media screen and (min-width: 600px) {
        padding: 111.1px 80px 60px 80px;
    }

    @media screen and (min-width: 1100px) {
        padding: 111.1px 90px 60px 90px;
    }
`

export const Heading = styled.h1`
    font-size: 30px;
    font-weight: 600;
    text-align: center;
    margin: 0;
`

export const ButtonWrapper = styled.div`
    display: flex;
    justify-content: center;
`