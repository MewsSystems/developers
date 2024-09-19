import { StyledNav, StyledLink, Logo, LogoText } from "./Header.styles";

const LogoUrl =
  "https://www.mews.com/hubfs/_Project_Phoenix/images/logo/Mews%20Logo.svg";

function Header() {
  return (
    <StyledNav>
      <StyledLink to="/">
        <Logo src={LogoUrl} alt="mewsflix logo"></Logo>
        <LogoText>FLIX</LogoText>
      </StyledLink>
    </StyledNav>
  );
}

export default Header;
