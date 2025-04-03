import styled from "styled-components";
import {Link} from "react-router-dom";

export const PageNotFound = () => {
    return (
        <NotFoundWrapper>
            <Heading>404</Heading>
            <SubHeading>Page Not Found</SubHeading>
            <StyledLink to="/">Go Back Home</StyledLink>
        </NotFoundWrapper>
    );
};

const NotFoundWrapper = styled.div`
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  height: 100vh;
  background-color: #f8f9fa;
  text-align: center;
  color: #333;
`;

const Heading = styled.h1`
  font-size: 6rem;
  margin: 0;
  color: #ff6b6b;
`;

const SubHeading = styled.h2`
  font-size: 2rem;
  margin: 10px 0;
`;

const StyledLink = styled(Link)`
  display: inline-block;
  margin-top: 20px;
  padding: 10px 20px;
  font-size: 1.2rem;
  color: white;
  background-color: #000;
  text-decoration: none;
  border-radius: 5px;
  transition: background-color 0.3s ease;

  &:hover {
    background-color: #000000d4;
  }
`;
