import { Link } from "react-router-dom";
import styled from "styled-components";
import { colors, device } from "../../utils/theme";
import Search from "../Search";

const StyledHeader = styled.header`
  width: 100%;
  text-align: center;
  @media ${device.mobileL} {
    display: flex;
    justify-content: space-between;
  }
`;

const StyledBoldHeading = styled.p`
  font-size: 30px;
  font-weight: 800;
  letter-spacing: 0.03rem;
  color: ${colors.primaryText};
`;

/**
 * Main header component of the application
 * @returns renders the header component
 */
const Header = () => {
  return (
    <>
      <StyledHeader>
        <Link style={{ textDecoration: "none" }} to={"/"}>
          <StyledBoldHeading>LOGO</StyledBoldHeading>
        </Link>
        <Search />
      </StyledHeader>
    </>
  );
};

export default Header;
