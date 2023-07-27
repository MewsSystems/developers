import { FC } from "react";
import Styled from "styled-components";
import NavigationBar from "../nav-bar/nav-bar";
const Title = Styled.h1`
  font-size: 1.5em;
  text-align: center;
  color: #ec701d;

`;

// Create a Wrapper component that'll render a <section> tag with some styles
const Wrapper = Styled.section`
  padding: 2em;
  background-color: #00ffd5;

`;

const HeaderComponent: FC<{}> = () => {
  return (
    <Wrapper>
      <Title>Find your movie !</Title>
      <NavigationBar></NavigationBar>
    </Wrapper>
  );
}

export default HeaderComponent;
