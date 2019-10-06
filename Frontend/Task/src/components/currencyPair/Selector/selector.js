import React, { Component } from 'react';
import { BootstrapTable, TableHeaderColumn } from 'react-bootstrap-table';

export default class Selector extends Component{

    rates = [];
    
    componentDidMount() {
        this.rates = this.props.rates;
    }
    
    returnValues(currency) {
        this.props.currencyList(currency);
    }
    
    prepareRates() {
        const rates = [];
        if(this.props.currencies != null) {
            for (const [key] of Object.entries(this.props.rates)) {
                const tmp = {
                    key: key,
                    name: this.props.currencies[key][0]["code"] + "/" + this.props.currencies[key][1]["code"],
                    display: this.props.rates[key]["display"] ? 'Yes':'No'
                }
                rates.push(tmp);
            }
        }
        return rates;
    }
    
    
    
    render(){
        const rates = this.prepareRates();
        const cellEditProp = {
            mode: 'click',
            blurToSave: true,
            afterSaveCell: (oldValue, newValue, row, column) => {
                if(row === "Yes") {
                    const currencyChange = {
                        currencyId: rates[column.rowIndex]["key"],
                        value: true
                    }
                    this.returnValues(currencyChange);
                } else {
                    const currencyChange = {
                        currencyId: rates[column.rowIndex]["key"],
                        value: false
                    }
                    this.returnValues(currencyChange);
                }
            }
        };
        return(
            <BootstrapTable data={rates} cellEdit={cellEditProp} striped>
                <TableHeaderColumn isKey dataField='name'>Market</TableHeaderColumn>
                <TableHeaderColumn dataField='display' editable={ { type: 'checkbox', options: { values: 'Yes:No' } } }>Visible</TableHeaderColumn>
            </BootstrapTable>
        )
    }
}