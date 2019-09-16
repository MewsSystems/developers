import React from 'react'
import { connect } from 'react-redux';
import { Rate, StringTMap, CurrencyPair, ApplicationState } from '@store/types';
import {
    Table,
    TableHeader,
    TableBody,
    TableCell,
    TextTableCell,
    HeaderTextTableCell,
    TableRowHeader
} from '../ui/table';
import RatesTableRow from './RatesTableRow';
import Trend from './Trend';
import { UrlParams } from 'containers/types';
import { RatesTableProps } from './types';
import { getFilteredState } from '@selectors/getFilteredState';
import NoFiltersResult from './NoFilterResults';

export interface PropsFromState {
    urlParams: UrlParams
    currencyPairs:  StringTMap<CurrencyPair>,
    currencyPairsIds: string[],
    rates: StringTMap<Rate>
}

const RatesTable: React.FC<PropsFromState> = ({ currencyPairs, currencyPairsIds, rates, urlParams }: PropsFromState) => (
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
            {currencyPairsIds.length == 0 && urlParams.searchTerm !== "" &&
                <NoFiltersResult searchTerm={urlParams.searchTerm} />
            }

            {currencyPairsIds && currencyPairsIds.length > 0 &&
                currencyPairsIds.map((id: string) => {
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


const mapStateToProps = ({currencyState, ratesState}: ApplicationState, props: RatesTableProps) => ({
    urlParams: props.urlParams,
    currencyPairs: currencyState.currencyPairs,
    currencyPairsIds: getFilteredState(currencyState, props),
    rates: ratesState.rates,
} as PropsFromState);

export default connect(
    mapStateToProps
)(RatesTable)