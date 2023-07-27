import { FC } from "react";
import { Link } from "react-router-dom";
import Styled from "styled-components";

const MainSection = Styled.section`
padding: 2em;
background: #919c9e;
`;

const StyledLink = Styled(Link)`
  color: #3f298d;
  font-weight: bold;
`;

const NotFound: FC<{}> = () => {
  return (
    <MainSection>
      <h2>It looks like you&apos;re lost...</h2>
      <p>
        <StyledLink to='/'>Go to the home page</StyledLink>
      </p>
    </MainSection>
  );
}

export default NotFound