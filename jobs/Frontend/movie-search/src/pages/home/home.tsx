import styled from "styled-components";
const Title = styled.h2`
font-size: 1.2em;
text-align: center;
color: #ec9a1d;
`;

// Create a Wrapper component that'll render a <section> tag with some styles
const MainSection = styled.section`
padding: 2em;
background: #919c9e;

`;

 function Home() {

  return (
   <MainSection>
    <Title>main</Title>

   </MainSection>
  );
}


export default Home;