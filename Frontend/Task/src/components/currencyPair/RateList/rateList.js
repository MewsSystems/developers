import React, { Component } from 'react';
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';

export default class RateList extends Component{

    prepareRates() {
        const rates = [];
        if(this.props.currencies != null) {
            for (const [key] of Object.entries(this.props.rates)) {
                if(this.props.rates[key]["display"] === true) {
                    const tmp = {
                        key: key,
                        name: this.props.currencies[key][0]["code"] + "/" + this.props.currencies[key][1]["code"],
                        value: this.props.rates[key]["trend"] + this.props.rates[key]["value"]
                    }
                    rates.push(tmp);
                }
            }
        }
        return rates;
    }
    render(){
        const rates = this.prepareRates();
        return(
            <BootstrapTable data={rates} striped>
                <TableHeaderColumn isKey dataField='name'>Market</TableHeaderColumn>
                <TableHeaderColumn dataField='value'>Price</TableHeaderColumn>
            </BootstrapTable>
        )
    }
}