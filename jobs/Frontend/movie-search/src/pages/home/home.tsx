import { FC, useEffect, useState } from "react";
import Styled, { keyframes } from "styled-components";
import CardsList from "../../components/card-list/card-list";
import { searchMoviesThunk, setQuery } from "../../store/movie-slice";
import { useAppDispatch, useAppSelector } from "../../store/store";
import spinner from "../../assets/512px-Spinner_font_awesome.png";

// Create a Wrapper component that'll render a <section> tag with some styles
const MainSection = Styled.section`
  padding: 2em 12em;
  background: #cad2d3;
`;
const InputStyled = Styled.input`
  width: 400px;
  height: 100%;
  padding-left: 12px;
  padding-right: 12px;
  border-radius: 10px;
  border: solid #ec701d;;
  color:#3f298d;
  caret-color:#ec701d;
  font-size:1em;
  outline: none;
  box-shadow: inset 0 -3px 0 rgba(0, 0, 0, 0.05);
  padding:10px;
  margin-bottom:16px;
`;
const NextPage = Styled.button`
  top:50vh;
  width:60px;
  height:60px;
  color:#3f298d;
  position:fixed;
  right:2vw;
  font-size:1em;
  border:none;
  pointer:click;
  border-radius:5px;
  background-color: #00ffd5;
  cursor: pointer; 
  &:hover{
    background-color: #97f1e2;
    color:#8370c9;
  }
  &:before{
    content:'>'
  }
`;
const PreviousPage = Styled(NextPage)`
  left:2vw;
  &:before{
    content:'<'
  }
`;

// Create the keyframes
const rotate = keyframes`
  from {
    transform: rotate(0deg);
  }

  to {
    transform: rotate(360deg);
  }
`;

const Spinner = Styled.div`
  display: block;
  animation: ${rotate} 2s linear infinite;
  padding: 2rem 1rem;
  font-size: 1.2rem;
`;

const StyledMessage = Styled.section`
  color:#3f298d;
  padding: 2em;
  text-align:center;
  height: 100vh;

`;

const INITAL_PAGE = 1;

const Home: FC<{}> = () => {
  const dispatch = useAppDispatch();
  const [query,setQueryState] = useState(useAppSelector((state) => state.movies.query));
  const [page, setPage] = useState(
    useAppSelector((state) =>
      state.movies.foundMoviesPage?.page
        ? state.movies.foundMoviesPage?.page
        : INITAL_PAGE
    )
  );
  const totalPages = useAppSelector(
    (state) => state.movies.foundMoviesPage?.totalPages
  );


  useEffect(() => {
      if(query!== "" && status==='init'){

        dispatch(searchMoviesThunk({ query: query, page: page }));
      }else if (status!=='init'){
        dispatch(searchMoviesThunk({ query: query, page: page }));
      }
    
    dispatch(setQuery({ query: query }));
  }, [query, page]);

  const handleChange = (value: { target: { value: string } }) => {
    console.log(value);

  
    setQueryState(value.target.value)
    setPage(INITAL_PAGE);
  };

  const handleClickNextPage = () => {
    console.log(page, totalPages);
    if (totalPages && totalPages > 1 && page) {
      let nextPage = page + 1;
      console.log(nextPage);
      setPage(nextPage);
    }
  };
  const handleClickPreviousPage = () => {
    if (totalPages && page > 1) {
      let previousPage = page - 1;
      setPage(previousPage);
    }
  };

  const status = useAppSelector((state) => state.movies.statusMoviesPage);

  const loadContent = () => {
    switch (status) {
      case "init":
        return <StyledMessage>Start to find movies! :D</StyledMessage>;

      case "loading":
        return (
          <Spinner>
            <img width={32} src={spinner} alt='spinner'></img>
          </Spinner>
        );
      case "empty":
        return <StyledMessage>Nothing found...</StyledMessage>;

      default:
        return <CardsList numberOfCards={10}></CardsList>;
    }
  };
  const loadButtons = () => {
    if (status === "idle") {
      return (
        <>
          <NextPage onClick={handleClickNextPage}></NextPage>
          <PreviousPage onClick={handleClickPreviousPage}></PreviousPage>
        </>
      );
    }
  };

  return (
    <MainSection className='home-container'>
      {loadButtons()}
      <InputStyled
        value={query}
        placeholder='Search your movie!'
        onChange={handleChange}
      ></InputStyled>
      {loadContent()}
    </MainSection>
  );
};

export default Home;
