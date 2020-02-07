import {
    BrowserRouter as Router,
    Switch,
    Route,
    Link
} from "react-router-dom";
import styled from 'styled-components'
import SearchView from '../views/SearchView';
import DetailsView from "../views/DetailsView";

import * as React from 'react';
export default function RouterComponent (){
    return (
        <Router>
            <Switch>
                <Route path="/details/:movieID?" render={(props) => <DetailsView propa={props}  />}/>
                <Route path="/" render={ () =><SearchView />} />
            </Switch>
        </Router>
    );

}
