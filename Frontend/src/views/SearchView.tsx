import * as React from 'react';
import * as actions from '../redux/moviesActions'
import styled from 'styled-components';
import {connect} from 'react-redux';
import Movie from "../components/Movie";
import Paginator from "../components/Paginator";

const Form = styled.form`
text-align:center;
& *{
  font-size: 24px;
  }
position: relative;
width:60%;
margin:0 auto;
`
const Input = styled.input`

position: relative;
box-sizing: border-box;
width:60%;
margin:0 auto;
margin-top: 10px;
`

export default connect(
    (store)=>{
        return {
            movies:store.moviesReducer,
            paginator: store.paginatorReducer,
        }
    },
)(
function SearchView (props){
    let maped;
    function handleSubmit(e){
        e.preventDefault();
        const searchPhrase = e.target.querySelector('input[name=searchPhrase]').value;
        props.dispatch(actions.fetchMovies(1,searchPhrase));
    }
    if(props.movies.fetched) {
        maped = props.movies.movies.map((movie) => {
            return <Movie key={movie.id} movie={movie} />
        })
    }
    function check(e) {
        console.log(e)
    }
    console.log(props.movies)
    return (
        <div>
            <Form onSubmit={(event)=>handleSubmit(event)}>
            <Input type="text" name="searchPhrase" default={props.movies.searchPhrase}/>
            <Input type="submit" value="search"/>
            </Form>
            <ul>
                { maped }
            </ul>
            <Paginator searchPhrase={props.movies.searchPhrase} pages={props.paginator}/>
        </div>
    )
}
)
