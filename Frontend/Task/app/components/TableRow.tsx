import React, { Component } from 'react';
import { CurrencyPair } from 'store/types';

interface TableRowProps {
    currencyPair: CurrencyPair,
    id: string
}

class TableRow extends Component<TableRowProps, {}> {
    constructor(props: TableRowProps) {
        super(props);
    }

    render() {
        const { currencyPair, id } = this.props;

        return (
            <div key={id} className="Table__Row">
                <span>{`${currencyPair[0].code}/${currencyPair[1].code}`}</span>
            </div>
        )
    }
}

export  default TableRow;