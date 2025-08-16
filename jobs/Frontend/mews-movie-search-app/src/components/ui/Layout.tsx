/* Global imports */
import * as React from "react";
import styled from "styled-components";

/* Local imports */

/* Types  */

/* Local utility functions */

/* Component definition */
export const Layout = ({ children }: { children: React.ReactNode }) => {
  return (
    <Flex>
      <HeaderNav>
        <Title color="#DFD0B8">Mews Movie Search App</Title>
      </HeaderNav>
      <MoviesContainer>{children}</MoviesContainer>
    </Flex>
  );
};

const Flex = styled.div`
  display: flex;
  height: 100vh;
  font-family: "Poppins", sans-serif;
  font-weight: 800;
  overflow: hidden;
  flex-direction: column;
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
`;

const MoviesContainer = styled.div`
  background: #3c5b6f;
`;

const Gap = styled.div`
  padding: 1rem;
`;
const Row = styled.div`
  display: flex;
  flex-direction: row;
  @media (max-width: 700px) {
    flex-direction: column;
    overflow-y: scroll;
  }
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
