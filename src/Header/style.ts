import styled from "styled-components";

export const HeaderWrapper = styled.header`
    background-color: #1c1c1c;
    padding: 20px 20px 20px 30px;
    position: fixed;
    top: 0;
    left: 0;
    right: 0;
    z-index: 1

    @media screen and (min-width: 600px) {
        padding-left: 80px;
    }

    @media screen and (min-width: 1100px) {
        padding-left: 90px;
    }
`

export const LogoWrapper = styled.a`
    width: 130px;
    display: block;
`

export const LogoImage = styled.img`
    width: 100%;
`