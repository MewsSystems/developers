import * as React from "react";
import styled from 'styled-components';
import * as actions from '../redux/moviesActions';
import {connect} from 'react-redux';

export default connect(
    (store)=>{
        return {
            movies:store.moviesReducer,
            paginator: store.paginatorReducer,
        }
    },
)(
function Paginator(props) {
    let previous=null;
    let next=null;
    console.log(props.pages)
    if(props.pages.currentPage > 1){
        previous = <div onClick={()=>props.dispatch(actions.fetchMovies(props.pages.currentPage-1,props.searchPhrase)) } >Previous</div>
    }
    if(props.pages.currentPage < props.pages.maxPage){
        next = <div onClick={()=>props.dispatch(actions.fetchMovies(props.pages.currentPage+1,props.searchPhrase)) } >Next</div>
    }
    return(
        <div>
            { previous }
            {props.pages.currentPage}
            { next }
        </div>
    )
}
)
