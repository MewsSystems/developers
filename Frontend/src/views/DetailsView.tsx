import { useEffect } from 'react';
import styled from 'styled-components';
import * as React from "react";
import { connect } from 'react-redux';
import {fetchDetails} from "../redux/detailsActions";
import MovieDetails from '../components/MovieDetails'

export default connect(
    (store)=>{
        return {
            details:store.detailsReducer,
        }
    },
)(
function DetailsView (props){
    let id = props.routerProps.match.params.movieID;
    let details = props.details.details;
    useEffect(() => {
        props.dispatch(fetchDetails(id));
    },[]);
    //console.log(props)
    let component = <div>WHOOPS! something went wrong</div>

    return (
        <div>
            {!details ? (component):(< MovieDetails history={props.routerProps.history} movieDetails={details}/>)}
        </div>
    )

}
)
