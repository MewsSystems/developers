import { FC } from "react";
import Styled from "styled-components";
import CardsList from "../../components/card-list/card-list";
import { searchMoviesThunk } from "../../store/movie-slice";
import { useAppDispatch } from "../../store/store";
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

  const dispatch = useAppDispatch();
  
  const handleChange = (value: { target: { value: string; }; })=>{
    console.log(value);
    
    dispatch(searchMoviesThunk(value.target.value))
  }


  return (
    <MainSection className="home-container">
      <Title >main</Title>
      <input onChange={handleChange} ></input>
      <CardsList numberOfCards={10} ></CardsList>
    </MainSection>
  );
}

export default Home;
