import link from 'react-router-dom';
import * as React from "react";
import styled from 'styled-components';

const Div = styled.div`
min-height: 12em;
  & >div{
  width: 90%;  
  display: inline-block;
  vertical-align: top;
  margin-top: 0px;  
  }
  & .title{
  font-size: 22px;
  width: 100%;
  text-align: center;
  padding-bottom: 8px;
  display: inline-block;  
  }
  & >image{
  min-width: 92px; 
  }
`

export default function Movie(props) {
    let movie = props.movie;
    return(
        <Div>
            <img alt="Smiley face" src={`http://image.tmdb.org/t/p/w92${movie.poster_path}`} />
            <div>
                <div className="title">{movie.title} </div>
                <div className="overview">{movie.overview}</div>
            </div>
        </Div>
    )
}
