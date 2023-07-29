import { FC } from "react";
import { Link, useNavigate } from "react-router-dom";
import { styled } from "styled-components";
import { getMovieDetailThunk } from "../../store/movie-thunks";
import { useAppDispatch } from "../../store/store";

const Wrapper = styled.div`
  background-color: rgb(245, 197, 24);
  ul,
  li {
    display: flex;
    text-decoration: none;
    list-style-type: none;
    justify-content: center;
    padding-left: 0px;
  }
`;

const StyledLink = styled(Link)`
  color: #3f298d;
  font-weight: bold;
  text-decoration: none;
  padding: 12px;
  width: 150px;
  border-radius: 5px;
  margin: 0px 8px;
  border: 2px solid #b94f08;
  background-color: #ec701d;
  &:hover {
    background-color: #f0b085; // <Thing> when hovered
  }
`;

const Styledanchor = styled.a`
  color: #3f298d;
  font-weight: bold;
  text-decoration: none;
  padding: 12px;
  width: 150px;
  border-radius: 5px;
  margin: 0px 8px;
  border: 2px solid #b94f08;
  background-color: #ec701d;
  &:hover {
    background-color: #f0b085; // <Thing> when hovered
  }
`;

const NavigationBar: FC<{}> = () => {
  const dispatch = useAppDispatch();
  const navigate = useNavigate();

  function randomIntFromInterval(min: number, max: number) {
    // min and max included
    return Math.floor(Math.random() * (max - min + 1) + min);
  }

  const handleClickViewDetail = () => {
    const id = randomIntFromInterval(200, 50000);
    navigate(`/movies/${id}`);

    dispatch(getMovieDetailThunk({ movieId: id }));
  };

  return (
    <Wrapper>
      <nav>
        <ul>
          <li>
            <StyledLink to='/'>Search</StyledLink>
          </li>
          <li>
            <Styledanchor onClick={handleClickViewDetail}>
              Watch tonight!
            </Styledanchor>
          </li>
          <li>
            <StyledLink to='/nothing-here'>Nothing Here</StyledLink>
          </li>
        </ul>
      </nav>
    </Wrapper>
  );
};

export default NavigationBar;
