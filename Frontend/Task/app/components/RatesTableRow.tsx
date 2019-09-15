import React, { Component, ReactNode } from 'react';
import { TableRow } from './ui/table/TableRow';
import { Rate } from '@store/types';

interface RatesTableRowProps {
    id: string;
    rate: Rate
}

class RatesTableRow extends Component<RatesTableRowProps, {}> {
    constructor(props: RatesTableRowProps) {
        super(props);
    }

    shouldComponentUpdate(nextProps: RatesTableRowProps) {
        /**
         * This is not deepCompare, so it won't work for nested object changes,
         * but this method is fast and it gives us speed improvement
         *
        */
        if (JSON.stringify(this.props.rate) !== JSON.stringify(nextProps.rate)) {
            return true;
        }

        return false;
    }

    render() {
        const { id, children } = this.props;

        return (
            <TableRow key={id}>
                {children}
            </TableRow>
        )
    }
}

export default RatesTableRow;