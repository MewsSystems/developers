import * as React from "react";
import styled from 'styled-components';
import { Link } from 'react-router-dom';
// @ts-ignore
import noimg from '../assets/noimg.png';
const LI = styled.li`
min-height: 12em;
width:100%;
  & >a{
  list-style: none;
  width: 100%;
  display: inline-block;
  border: 1px solid green;
  }
  & >a>div{
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
    const movie = props.movie;
    let img=<img alt="xd"></img>;
    if(movie.poster_path){
        img = <img alt="Smiley face" src={`http://image.tmdb.org/t/p/w92${movie.poster_path}`} />
    }
    else {
        img = <img alt="Smiley face" src={noimg} />
    }
    return(
        <LI>
            <Link to={`/details/${movie.id}`} >
                { img }
                <div>
                    <div className="title">{movie.title} </div>
                    <div className="overview">{movie.overview}</div>
                </div>
            </Link>
        </LI>
    )
}


