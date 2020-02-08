import link from 'react-router-dom';
import * as React from "react";
import styled from 'styled-components';
//import * as url from '../assets/noimg.png' ;
const noimg =require('../assets/noimg.png');
const Div = styled.div`
min-height: 12em;
  & >div{
  padding: 8px 8px 8px 8px;
  width: 80%;  
  min-width: 400px;
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
  & img{
  width: 10%;
  min-width: 92px; 
  height: auto;
  }
`

export default function Movie(props) {
    let movie = props.movie;
    let img=<img alt="xd"></img>;
    if(movie.poster_path){
        img = <img alt="Smiley face" src={`http://image.tmdb.org/t/p/w92${movie.poster_path}`} />
    }
    else {
        img = <img alt="Smiley face" src={noimg} />
    }
    return(
        <Div>
            { img }
            <div>
                <div className="title">{movie.title} </div>
                <div className="overview">{movie.overview}</div>
            </div>
        </Div>
    )
}
