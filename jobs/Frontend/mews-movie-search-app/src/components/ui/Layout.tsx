/* Global imports */
import * as React from "react";
import styled from "styled-components";

/* Local imports */

/* Types  */

/* Local utility functions */

/* Component definition */
export const Layout = ({ children }: { children: React.ReactNode }) => {
  return (
    <Grid>
      <HeaderNav>
        <Title color="#DFD0B8">Mews Movie Search App</Title>
      </HeaderNav>
      <MoviesContainer>{children}</MoviesContainer>
    </Grid>
  );
};

const Grid = styled.div`
  display: grid;
  height: 100vh;
  font-family: "Poppins", sans-serif;
  font-weight: 800;
  overflow: hidden;
  grid-template-rows: 0.5fr 0.5fr auto;
  grid-template-areas:
    "nav nav nav nav"
    "search search search search"
    "content content content content";
`;

const Title = styled.h3<{ color?: string }>`
  color: ${(props) => props.color ?? "white"};
  padding: 0.5rem 1rem;
`;
const HeaderNav = styled.nav`
  background: #4f4a45;
  justify-content: flex-start;
  display: flex;
  flex-direction: column;
  width: 100%;
  grid-area: nav;
`;

const MoviesContainer = styled.div`
  background: #3c5b6f;
  grid-area: content;
`;

const Gap = styled.div`
  padding: 1rem;
`;
const Row = styled.div`
  display: flex;
  flex-direction: row;
`;
const Column = styled.div`
  display: flex;
  flex-direction: column;
`;
const Separator = styled.hr`
  border: 0px;
  height: 1px;
  background: rgba(0, 0, 0, 0.5);
  width: 100%;
`;

const Wrapper = styled.div`
  margin: 1rem 0px;
  display: flex;
  justify-content: space-between;
  align-items: center;
`;

const Text = styled.span<{
  color?: string;
  size?: "base" | "xs" | "sm" | "md" | "lg" | "xl";
}>`
  color: ${(props) => props.color ?? "black"};
  font-size: ${(props) => {
    switch (props.size) {
      case "xs":
        return "0.25rem";
      case "sm":
        return "0.5rem";
      case "md":
        return "0.75rem";
      case "lg":
        return "1.5rem";
      case "xl":
        return "3rem";
      default:
        return "1rem";
    }
  }};
`;

export { Wrapper, Title, Text, Gap, Row, Column, Separator };
