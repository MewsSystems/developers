"use client";

import { tabletMediaQuery } from "@/breakpoints";
import { useRouter } from "next/navigation";
import styled from "styled-components";

const Container = styled.div`
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
