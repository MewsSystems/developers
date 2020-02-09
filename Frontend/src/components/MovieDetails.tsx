import * as React from 'react';
import styled from 'styled-components';

const DIV = styled.div`
margin: 5% 5% 5% 5%;
display: flex;
flex-flow: row wrap;
justify-content: space-around;
  & div{  
  max-width: 700px;
  min-width: 200px;
  } 
  & img{
  width: 100%;
  max-width: 500px;
  }
`

const BUTTON = styled.button`
margin: 0px auto;
margin-top: 30px;
font-size: 24px;
min-width: 400px;
display:block;
`
export default function MovieDetails(props) {
    let movieDetails = props.movieDetails;
    console.log(movieDetails,props);

    let genresString = movieDetails.genres.reduce((acumulator,genre)=>{
        return ` ${genre.name}${acumulator}`;
    },"");
    let productionCountries = movieDetails.production_countries.reduce((acumulator,country)=>{
       return `${country.name}, ${acumulator}`;
    },"");
    let productionCompanies = movieDetails.production_companies.reduce((acumulator,company)=>{
        return `${acumulator}${company.name}, `;
    },"");

    return(
        <div>
            <BUTTON
                className="button icon-left"
                onClick={props.history.goBack}>
                Back
            </BUTTON>
            <DIV>
                <div>
                    <img src={`http://image.tmdb.org/t/p/w500${movieDetails.poster_path}`}></img>
                </div>
                <div>
                    <h1>{movieDetails.title}</h1>
                    <div> release date: {movieDetails.release_date}  </div>
                    <div>genre: {genresString}</div>
                    <div>production: { productionCountries}</div>
                    <div>producers: { productionCompanies}</div>
                    <p>{movieDetails.tagline}</p>
                    <p>{movieDetails.overview}</p>
                </div>
            </DIV>

        </div>
    )
}
