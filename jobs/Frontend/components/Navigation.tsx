"use client";

import { useRouter } from "next/navigation";
import Link from "next/link";
import styled, { css } from "styled-components";
import { tabletMediaQuery } from "@/breakpoints";

const Container = styled.nav`
  padding: 10px;

  ${tabletMediaQuery} {
    margin-left: 50px;
  }
`;

const sharedStyled = css`
  background: ${(props) => props.theme.primary.button};
  color: ${(props) => props.theme.secondary.text};
  border-radius: 8px;
  height: 45px;
  width: 150px;
  font-size: 14px;

  &:hover {
    cursor: pointer;
    background: ${(props) => props.theme.primary.buttonHover};
  }
`;

const StyledButton = styled.button`
  ${sharedStyled}
`;

const StyledLink = styled(Link)`
  ${sharedStyled}
  display: flex;
  justify-content: center;
  align-items: center;
  text-decoration: none;
`;

interface NavigationProps {
  query: string;
  page: string;
}

const Navigation = ({ query, page }: NavigationProps) => {
  const router = useRouter();

  // If you land directly on a movie details page you won't have a search to go back to, instead go to homepage
  return (
    <Container>
      {query && page ? (
        <StyledButton onClick={() => router.back()}>
          Back to Search
        </StyledButton>
      ) : (
        <StyledLink href={"/"}>Home</StyledLink>
      )}
    </Container>
  );
};

export default Navigation;
