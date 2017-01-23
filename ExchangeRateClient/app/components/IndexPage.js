'use strict';

import React from 'react';
import {BootstrapTable, TableHeaderColumn} from 'react-bootstrap-table';
import axios from 'axios';

import config from '../config/config.json';
import styles from '../static/css/rate.css';

const trendType = {
    0: 'growing',
    1: 'declining',
    2: 'stagnating'
};

function enumFormatter(cell, row, enumObject) {
    return enumObject[cell];
}

export default class IndexPage extends React.Component {

    constructor(props) {
        super(props);

        this.state = {
            allIDs : [],
            currencyPairs: []
        };

        this.getAllRates = this.getAllRates.bind(this);
    }

    getInitialState() {
        return {
            allIDs : [],
            currencyPairs: []
        };
    }

    componentDidMount() {
        // Get Configuration
        axios.get(
            config.endPoint + '/configuration',
            {}
        )
        .then(response => {
            var pairs = [];

            var allIDs = [];
            Object.keys(response.data.currencyPairs).forEach(function(key) {
                allIDs.push(key);

                var row = {};
                row['id'] = key;
                row['code_1'] = response.data.currencyPairs[key][0]['code'];
                row['name_1'] = response.data.currencyPairs[key][0]['name'];
                row['code_2'] = response.data.currencyPairs[key][1]['code'];
                row['name_2'] = response.data.currencyPairs[key][1]['name'];
                row['pair_name'] = row['name_1'] + "   /   " + row['name_2'];
                row['pair_code'] = row['code_1'] + "   /   " + row['code_2'];
                row['rate_old'] = 0;
                row['rate_new'] = 0;
                row['trend'] = '';

                pairs.push(row);
            });

            this.setState({ allIDs:allIDs, currencyPairs:pairs });
            this.getAllRates();

            // Get All Rates
            this.timer = setInterval(this.getAllRates, config.interval);
        })
        .catch(error => {
            console.log(error);
        });
    }

    getAllRates() {
        var pairs = this.state.currencyPairs;
        axios.get(
                config.endPoint + '/rates',
                {params : { 'currencyPairIds' : this.state.allIDs }}
            )
            .then(response => {
                console.log(response.data.rates);

                for(var i = 0; i < this.state.allIDs.length; i++) {
                    pairs[i]['rate_old'] = pairs[i]['rate_new'];
                    pairs[i]['rate_new'] = response.data.rates[this.state.allIDs[i]];

                    if(Number(pairs[i]['rate_old']) < Number(pairs[i]['rate_new'])) {
                        pairs[i]['trend'] = '0';
                    }
                    else if(Number(pairs[i]['rate_old']) > Number(pairs[i]['rate_new'])) {
                        pairs[i]['trend'] = '1';
                    }
                    else {
                        pairs[i]['trend'] = '2';
                    }
                }

                this.setState({ currencyPairs:pairs });
            })
            .catch(error => {
                console.log(error);
            });
    }

    render() {
        return (
            <div className="rate_list">
            <BootstrapTable data={this.state.currencyPairs} striped hover options={ { noDataText: 'Please wait while loading data...' } }>
                <TableHeaderColumn width="250" isKey dataField='pair_name' filter={ { type: 'RegexFilter', delay: 500 } }>Pair Name</TableHeaderColumn>
                <TableHeaderColumn width="100" dataField='pair_code' filter={ { type: 'RegexFilter', delay: 500 } }>Pair Code</TableHeaderColumn>
                <TableHeaderColumn width="80" dataField='rate_old'>Old Rate</TableHeaderColumn>
                <TableHeaderColumn width="80" dataField='rate_new'>New Rate</TableHeaderColumn>
                <TableHeaderColumn width="80" dataField='trend' filterFormatted dataFormat={ enumFormatter }
                                   formatExtraData={ trendType } filter={ { type: 'SelectFilter', options: trendType } }>Trend</TableHeaderColumn>
            </BootstrapTable>
            </div>
        );
    }
}
