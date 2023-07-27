import styled from "styled-components";
import NavigationBar from "../nav-bar/nav-bar";
const Title = styled.h1`
  font-size: 1.5em;
  text-align: center;
  color: #ec9a1d;
`;

// Create a Wrapper component that'll render a <section> tag with some styles
const Wrapper = styled.section`
  padding: 2em;
  background: #77d1e0;
`;

export default function HeaderComponent() {
  return (
    <Wrapper>
      <Title>Find your movie !</Title>
      <NavigationBar></NavigationBar>
    </Wrapper>
  );
}
