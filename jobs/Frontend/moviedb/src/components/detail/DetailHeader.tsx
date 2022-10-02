import React from 'react';
import styled, { css } from 'styled-components';
import { Container } from '../../styles/Container.styled';
import { createImgPath } from '../../utils';

const Header = styled.header`  
  background: no-repeat #212121 center / cover;
  color: #fff;
  display: flex;
  flex-direction: column;
  justify-content: center;
  font-size: 22px;
  margin-bottom: 60px;
  min-height: 70vh;
  padding: 20px 0;
  
  ${(props: { bg: string | false }) => props.bg && css`
    background-image: url(${props.bg});
    position: relative;
    
    &::before {
      background: #000;
      content: "";
      opacity: .5;
      position: absolute;
      left: 0;
      right: 0;
      top: 0;
      bottom: 0;
    }
    
    h1,
    p {
      position: relative;
    }
  `}
`;

interface IMovieDetailHeaderProps {
  movie: IFetchMovieDetail
}

export default function DetailHeader({ movie }: IMovieDetailHeaderProps) {
  const bg = movie.backdrop_path ? createImgPath(movie.backdrop_path) : false;

  return (
    <Header bg={bg}>
      <Container>
        <h1>{movie.title}</h1>
        <p>{movie.tagline}</p>
      </Container>
    </Header>
  );
}
