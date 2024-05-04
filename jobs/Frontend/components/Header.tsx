"use client";

import { ReactNode } from "react";
import styled from "styled-components";

const Container = styled.div`
  border-bottom: 1px solid ${(props) => props.theme.primary.border};
`;

interface HeaderProps {
  children?: ReactNode;
}

const Header = ({ children }: HeaderProps) => {
  return <Container>{children}</Container>;
};

export default Header;
