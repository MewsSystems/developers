import { FC } from "react";
import Styled from "styled-components";
import CardsList from "../../components/card-list/card-list";
const Title = Styled.h2`
  font-size: 1.2em;
  text-align: center;

`;

// Create a Wrapper component that'll render a <section> tag with some styles
const MainSection = Styled.section`
  padding: 2em 12em;
  background: #cad2d3;
`;

const Home: FC<{}> = () => {

  return (
    <MainSection className="home-container">
      <Title>main</Title>
      <CardsList numberOfCards={10} ></CardsList>
    </MainSection>
  );
}

export default Home;
