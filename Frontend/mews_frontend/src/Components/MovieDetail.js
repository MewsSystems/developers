import React, { useEffect } from 'react';
import styled from 'styled-components';
import { useParams } from "react-router-dom";
import { connect } from 'react-redux';
import { fetchMovie } from '../actions/movieAction';

const Container = styled.div`

width:90%;
margin:0 auto;
`;
const Wrapper = styled.div`
margin-top:2em;
display:flex;
@media (max-width: 768px) {
    flex-direction: column;
    
  }

`;
const Image = styled.img`
  border: 1px solid #fff;
  padding: 1em;
  width:20em;
  margin-right: 2em;
  flex:0.3;
  @media (max-width: 768px) {
    width:100%;
    
  }

`;
const Info = styled.div`

flex:0.7;
`;

const Ul = styled.ul`
`;
const H1 = styled.h1`
margin: 0.5em 0;
border-bottom:1px solid #fff;
display:inline-block;
`;

const Li = styled.li`

border: 1px solid #fff;
padding: 0.5em;
margin-bottom:1em;
margin-top:1em;
text-align:left;

`;
const base_url = 'https://image.tmdb.org/t/p/original/';

const MovieDetail = ({ movie, fetchMovie }) => {
    let { id } = useParams();

    useEffect(() => {
        fetchMovie(id);
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [])


    return (
        <Container>
            <Wrapper>
                <Image src={base_url + movie.poster_path} alt="" />
                <Info>
                    <H1>{movie.title}</H1>
                    <Ul>
                        <Li>
                            <strong>Gernre :</strong> {movie.genres[0].name}
                        </Li>
                        <Li>
                            <strong>Released :</strong> {movie.release_date}
                        </Li>
                        <Li>
                            <strong>Vote Average :</strong> {movie.vote_average}
                        </Li>

                        <Li>
                            <strong>Revenue :</strong> $ {movie.revenue}
                        </Li>

                        <Li>
                            <strong>Overview :</strong> {movie.overview}
                        </Li>
                    </Ul>
                </Info>
            </Wrapper>

        </Container>
    )
}
const mapStateToProps = state => ({
    movie: state.movie.movie,
    loading: state.movie.loading

})
export default connect(mapStateToProps, { fetchMovie })(MovieDetail)
