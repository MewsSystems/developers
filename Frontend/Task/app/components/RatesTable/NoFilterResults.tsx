import React from 'react'
import {
    TableCell,
    TextTableCell,
    TableRow
} from '../ui/table';

export interface NoFiltersResultProps {
    searchTerm: string
}

const NoFiltersResult: React.FC<NoFiltersResultProps> = ({ searchTerm }: NoFiltersResultProps) => (
    <TableRow>
        <TableCell>
            <TextTableCell style={{textAlign: 'center', fontWeight: 'bold'}}>No searching results for "{searchTerm}"</TextTableCell>
        </TableCell>
    </TableRow>
);

NoFiltersResult.displayName = 'NoFiltersResult';

export default NoFiltersResult;