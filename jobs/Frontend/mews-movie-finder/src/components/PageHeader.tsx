import styled from "styled-components";
import { VStack } from "./Stacks";
import logo from "../assets/tsc.png";

const HeaderWrapper = styled.div`
  padding: 8px;
  margin-bottom: 8px;
  border-bottom: solid whitesmoke 1px;
  display: flex;
  flex-direction: row;
`;

const Title = styled.h1`
  font-family: Exo;
  font-size: 32px;
  font-weight: normal;
  margin: 0;
`;

const SubTitleWithLink = styled.a`
  font-family: Exo;
  font-size: 12px;
  font-weight: normal;
  margin: 0;
  color: whitesmoke;
  text-decoration: none;
  &:hover {
    text-decoration: underline;
    color: whitesmoke;
  }
`;

const Logo = styled.img`
  width: 128px;
  margin-right: 6px;
`;

export function PageHeader() {
  return (
    <HeaderWrapper>
      <Logo src={logo} />
      <VStack $alignItems="baseline">
        <Title>Movie Finder</Title>
        <SubTitleWithLink href="https://armory.thespacecoder.space/">
          by TheSpaceCoder
        </SubTitleWithLink>
      </VStack>
    </HeaderWrapper>
  );
}
