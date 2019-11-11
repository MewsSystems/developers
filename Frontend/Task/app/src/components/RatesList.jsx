import React from 'react'

import Table from '@material-ui/core/Table';
import TableBody from '@material-ui/core/TableBody';
import TableCell from '@material-ui/core/TableCell';
import TableHead from '@material-ui/core/TableHead';
import TableRow from '@material-ui/core/TableRow';
import TrendIcon from "./TrendIcon";

const RatesList = (props) => {
    const {config, currencyPairsRates, filter} = props;
    const actualRates = currencyPairsRates.length > 0 ? currencyPairsRates[currencyPairsRates.length - 1] : null;
    const lastRates = currencyPairsRates.length > 1 ? currencyPairsRates[currencyPairsRates.length - 2] : null;

    return (
        <Table>
            <TableHead>
                <TableRow>
                    <TableCell>Currency pairs</TableCell>
                    <TableCell align="right">Rate</TableCell>
                    <TableCell align="right">Trend</TableCell>
                </TableRow>
            </TableHead>
            <TableBody>
                {Object.keys(config).filter(value => {
                    if (filter === '') {
                        return true;
                    }

                    const currencyPair = config[value][0].code + config[value][1].code;

                    return currencyPair
                        .toLowerCase()
                        .includes(
                            filter.replace(/\//g, '')
                                .replace(/\s+/g, '')
                                .toLowerCase()
                        );
                }).map(key => (
                    <TableRow key={key}>
                        <TableCell>{config[key][0].code} / {config[key][1].code}</TableCell>
                        <TableCell align="right">{actualRates ? actualRates[key] : '-'}</TableCell>
                        <TableCell align="right">
                            <TrendIcon
                                actualRate={actualRates ? actualRates[key] : null}
                                lastRate={lastRates ? lastRates[key] : null}
                            />
                        </TableCell>
                    </TableRow>
                ))}
            </TableBody>
        </Table>
    )
};

export default RatesList;