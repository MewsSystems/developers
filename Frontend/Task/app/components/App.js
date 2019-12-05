import React, { Component, Fragment } from 'react';
import RateList from './RateList';

export default class App extends Component {
    render(){
        return (
            <Fragment>
                <h1>Rates list</h1>
                <RateList />
            </Fragment>
        );
    }
}