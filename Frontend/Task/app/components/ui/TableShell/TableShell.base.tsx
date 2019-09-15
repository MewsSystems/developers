import React, { PureComponent, HTMLAttributes } from 'react';

import { Word } from '@components/ui/Shell/Word';
import { Paragraph } from '@components/ui/Shell/Paragraph';
import { TableShellContainer } from './TableShell.styled';

export interface TableShellProps extends HTMLAttributes<HTMLDivElement> {
    className?: string;
    filtersBottomSpace: number;
    rows?: number;
    cols?: number;
}

class TableShell extends PureComponent<TableShellProps> {
    static defaultProps: Partial<TableShellProps> = {
        filtersBottomSpace: 20,
        rows: 5,
        cols: 5
    };
    render() {
        const { rows, cols } = this.props;

        return (
            <TableShellContainer className="TableShell Table">
                <div className="TableShell__Header Table__Header">
                    <div className="Table__Row Table__Row--header">
                        <div className="Table__Cell Table__Cell--header Table__Cell--date">
                            <Word letters={6} spaceAfter={true} />
                        </div>
                    </div>
                </div>
                <div className="Table__Body">
                    {[...Array(rows)].map((j,k) =>  <div key={`row-${k}`} className="Table__Row Table__Row--body">
                        {[...Array(cols)].map((i, k) => <div key={`cell-${k}`} className='Table__Cell Table__Cell--body'>
                                <Paragraph words={1} />
                            </div>
                        )}
                        </div>
                    )}
                </div>
            </TableShellContainer>
        );
    }
}

export default TableShell;