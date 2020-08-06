import styled from 'styled-components';
import { Link } from 'react-router-dom';

export const Title = styled.h2`
  text-align: center;
`;

export const SearchResultsWrapper = styled.div`
  display: flex;
  flex-wrap: wrap;
`;

export const SearchInput = styled.input`
  display: block;
  width: 80%;
  margin: 2rem auto 0;
  font-size: 2rem;
  padding: 0.4rem;
`;

export const MovieLink = styled(Link)`
  display: flex;
  width: 20%;
  margin: 2.5%;
  color: #fff;
  justify-content: center;
  align-items: center;

  @media only screen and (max-width: 768px) {
    width: 45%;
  }
`;

export const MoviePosterImage = styled.img`
  display: block;
  width: 100%;
`;
