import React from 'react'

import { Rate, StringTMap, CurrencyPair } from '@store/types';
import RatesTableRow from './RatesTableRow';
import { Table, TableHeader, TableBody, TableCell, TextTableCell, HeaderTextTableCell, TableRowHeader } from './ui/table';
import Trend from './Trend';

interface RatesTableProps {
    currencyPairs:  StringTMap<CurrencyPair>,
    currencyPairsIdList: string[],
    rates: StringTMap<Rate>
}

const RatesTable: React.FC<RatesTableProps> = ({ currencyPairs, currencyPairsIdList, rates }: RatesTableProps) => (
    <Table>
        <TableHeader>
            <TableRowHeader>
                <TableCell>
                    <HeaderTextTableCell>Pair</HeaderTextTableCell>
                </TableCell>
                <TableCell>
                    <HeaderTextTableCell>Value</HeaderTextTableCell>
                </TableCell>
                <TableCell>
                    <HeaderTextTableCell>Trend</HeaderTextTableCell>
                </TableCell>
            </TableRowHeader>
        </TableHeader>
        <TableBody>
            {currencyPairsIdList && currencyPairsIdList.length > 0 &&
                currencyPairsIdList.map((id: string) => {
                    return (
                        <RatesTableRow key={id} id={id} rate={rates[id]}>
                            <TableCell>
                                <TextTableCell>{`${currencyPairs[id][0].code}/${currencyPairs[id][1].code}`}</TextTableCell>
                            </TableCell>
                            <TableCell>
                                <TextTableCell>{rates[id] && rates[id].value || "—"}</TextTableCell>
                            </TableCell>
                            <TableCell>
                                <TextTableCell><Trend rate={rates[id]}/></TextTableCell>
                            </TableCell>
                        </RatesTableRow>
                    )
                })
            }
        </TableBody>
    </Table>
);

RatesTable.displayName = 'RatesTable';

export default RatesTable;