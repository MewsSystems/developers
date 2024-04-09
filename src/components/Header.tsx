import styled from "styled-components";
import mewsLogo from "../assets/mews-logo.svg";

const HeaderContainer = styled.header`
    background-color: var(--deep-blue);
    height: 100px;
    display: flex;
    align-items: center;
    flex-direction: column;
    justify-content: center;
`;

const Logo = styled.img`
    width: 120px;
    height: 15px;
`;

const LogoLink = styled.a`
    display: flex;
    flex-direction: row;
    align-items: baseline;
    justify-content: center;
`;

const LogoText = styled.h2`
    font-family: "Axiforma-Light", sans-serif;

    span {
        font-family: "Axiforma-Bold", sans-serif;
    }
`;

export const Header = () => {
    return (
        <HeaderContainer className="bg-gray-800 text-white py-4 w-full">
            <LogoLink href="/">
                <Logo
                    className="image h-full w-full object-contain"
                    src={mewsLogo}
                    alt="Mews Logo"
                />
                <em>vies</em>
            </LogoLink>
            <LogoText className="text-xs xs:text-base mt-1">
                movies app from <span className="font-bold">themoviedb API</span>
            </LogoText>
        </HeaderContainer>
    );
};
