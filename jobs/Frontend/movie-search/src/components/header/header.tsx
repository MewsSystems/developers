import { FC } from "react";
import Styled from "styled-components";
import { useAppSelector } from "../../store/store";
import NavigationBar from "../nav-bar/nav-bar";
const Title = Styled.h1`
  font-size: 1.5em;
  text-align: center;
  color: #ec701d;

`;

const PageCounter = Styled.div`
  font-size: 1em;
  text-align: center;
  color: #ec701d;
  padding:0;
  min-height:21px;
`;

// Create a Wrapper component that'll render a <section> tag with some styles
const Wrapper = Styled.section`
  padding: 2em;
  background-color: #00ffd5;
`;

const HeaderComponent: FC<{}> = () => {
  const totalPages = useAppSelector(
    (state) => state.movies.foundMoviesPage?.totalPages
  );
  const status = useAppSelector((state) => state.movies.statusMoviesPage);

  const total = useAppSelector((state) => state.movies.foundMoviesPage?.total);

  const pages = useAppSelector((state) => state.movies.foundMoviesPage?.page);

  return (
    <Wrapper>
      <Title>Find your movie !</Title>
      <NavigationBar></NavigationBar>
      <PageCounter>
        {total !== 0 && status !== "init" ? `Page: ${pages}/${totalPages}` : ""}
      </PageCounter>
    </Wrapper>
  );
};

export default HeaderComponent;
