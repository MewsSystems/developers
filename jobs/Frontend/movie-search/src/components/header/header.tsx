import { FC } from "react";
import { useLocation } from "react-router-dom";
import Styled from "styled-components";
import { useAppSelector } from "../../store/store";
import NavigationBar from "../nav-bar/nav-bar";
const Title = Styled.h1`
  font-size: 1.5em;
  text-align: center;
  color:#3f298d;


`;

const PageCounter = Styled.div`
  font-size: 1em;
  text-align: center;
  color:#3f298d;
  padding:0;
  min-height:21px;
`;

// Create a Wrapper component that'll render a <section> tag with some styles
const Wrapper = Styled.section`
  padding: 2em;
  background-color:  rgb(245, 197, 24);
`;

const HeaderComponent: FC<{}> = () => {
  let location = useLocation();

  const totalPages = useAppSelector(
    (state) => state.movies.foundMoviesPage?.totalPages
  );
  const status = useAppSelector((state) => state.movies.statusMoviesPage);

  const total = useAppSelector((state) => state.movies.foundMoviesPage?.total);

  const pages = useAppSelector((state) => state.movies.foundMoviesPage?.page);

  const getPageCounter = (): string => {
    console.log(location,location.pathname.includes('/'));
    if (total !== 0 && status !== "init" && !(location.pathname.includes('movies') || location.pathname.includes('nothing'))) {
      return `Page: ${pages}/${totalPages}`;
    } else {
      return "";
    }
  };

  return (
    <Wrapper>
      <Title>Find your movie!</Title>
      <NavigationBar></NavigationBar>
      <PageCounter>{getPageCounter()}</PageCounter>
    </Wrapper>
  );
};

export default HeaderComponent;
