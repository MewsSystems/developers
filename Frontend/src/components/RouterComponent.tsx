import {
    BrowserRouter as Router,
    Switch,
    Route,
    Link
} from "react-router-dom";
var browserHistory = Router.browserHistory;


import SearchView from '../views/SearchView';
import DetailsView from "../views/DetailsView";

import * as React from 'react';
export default function RouterComponent (){
    return (
        <Router history={browserHistory}>
            <Switch>
                <Route path="/details/:movieID?" render={(props) => <DetailsView routerProps={props}  />}/>
                <Route path="/" render={ () =><SearchView />} />
            </Switch>
        </Router>
    );

}
