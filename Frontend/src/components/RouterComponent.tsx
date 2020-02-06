import {
    BrowserRouter as Router,
    Switch,
    Route,
    Link
} from "react-router-dom";
import SearchView from '../views/SearchView';
import DetailsView from "../views/DetailsView";
import * as React from 'react';
export default function RouterComponent (){
    return (
        <Router>
            <Switch>
                <Route path="/details/:movieID?" render={(props) => <DetailsView {...props} isAuthed={true} />}/>
                <Route path="/" component={SearchView}/>
            </Switch>
        </Router>
    );

}
