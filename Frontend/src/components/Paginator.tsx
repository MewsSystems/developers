import * as React from "react";
import styled from 'styled-components';
import * as actions from '../redux/moviesActions';
import {connect} from 'react-redux';

const DIV = styled.div`
margin: 0 auto;
margin-bottom: 50px;
width: 400px;
  & button{
  width: 100%;
  }

    & >div{
    display: inline-block;
    width: 33%;
    text-align: center;
    }
`

export default connect(
    (store)=>{
        return {
            movies:store.moviesReducer,
            paginator: store.paginatorReducer,
        }
    },
)(
function Paginator(props) {
    let previous= null;
    let next= null;
    console.log(props.pages)
    if(props.pages.currentPage > 1){
        previous = <button onClick={()=>props.dispatch(actions.fetchMovies(props.pages.currentPage-1,props.searchPhrase)) } >Previous</button>
    }
    if(props.pages.currentPage < props.pages.maxPage){
        next = <button onClick={()=>props.dispatch(actions.fetchMovies(props.pages.currentPage+1,props.searchPhrase)) } >Next</button>
    }
    return(
        <DIV>
            <div>{ previous }</div>
            <div>{props.pages.currentPage}</div>
            <div>{ next }</div>
        </DIV>
    )
}
)
