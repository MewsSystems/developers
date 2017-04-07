'use strict';

import React from 'react'
import { Route, IndexRoute } from 'react-router'
import Layout from './Layout';
import IndexPage from './IndexPage';
import NotFoundPage from './NotFound';

const routes = (
    <Route path="/" component={Layout}>
        <IndexRoute component={IndexPage}/>
        <Route path="*" component={NotFoundPage}/>
    </Route>
);

export default routes;
