import styled from "styled-components";

const StyledLink = styled.a`
  color: ${props => props.theme.colors.primary.on};
`;

const StyledWrapper = styled.div`
  width: 100vw;
  height: 100vh;
  background-color: ${props => props.theme.colors.primary.main};
`;

export function Search() {
  return (
    <StyledWrapper>
      Movie search
      <StyledLink href="/details/123">Go to the details page</StyledLink>
    </StyledWrapper>
  );
}
