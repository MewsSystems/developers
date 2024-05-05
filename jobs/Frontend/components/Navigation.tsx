"use client";

import { useRouter } from "next/navigation";
import styled from "styled-components";
import { tabletMediaQuery } from "@/breakpoints";

const Container = styled.nav`
  padding: 10px;

  ${tabletMediaQuery} {
    margin-left: 50px;
  }
`;

const Button = styled.button`
  background: ${(props) => props.theme.primary.button};
  color: ${(props) => props.theme.secondary.text};
  border-radius: 8px;
  height: 30px;
  width: 150px;

  &:hover {
    cursor: pointer;
    background: ${(props) => props.theme.primary.buttonHover};
  }
`;

const Navigation = () => {
  const router = useRouter();

  return (
    <Container>
      <Button onClick={() => router.back()}>Back to Search</Button>
    </Container>
  );
};

export default Navigation;
