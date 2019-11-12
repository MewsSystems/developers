import React from 'react'

import Table from '@material-ui/core/Table';
import TableBody from '@material-ui/core/TableBody';
import TableCell from '@material-ui/core/TableCell';
import TableHead from '@material-ui/core/TableHead';
import TableRow from '@material-ui/core/TableRow';
import TrendIcon from "./TrendIcon";
import {makeStyles} from "@material-ui/core";

const useStyles = makeStyles(theme => ({
    th: {
        fontWeight: 'bold',
    }
}));

const matchFilter = (value, filter) => {
    return value
        .toLowerCase()
        .includes(
            filter.replace(/\//g, '')
                .replace(/\s+/g, '')
                .toLowerCase()
        );
};

const RatesList = (props) => {
    const {config, currencyPairsRates, filter} = props;
    const actualRates = currencyPairsRates.length > 0 ? currencyPairsRates[currencyPairsRates.length - 1] : null;
    const lastRates = currencyPairsRates.length > 1 ? currencyPairsRates[currencyPairsRates.length - 2] : null;
    const classes = useStyles();

    return (
        <Table>
            <TableHead>
                <TableRow>
                    <TableCell className={classes.th}>Name</TableCell>
                    <TableCell className={classes.th}>Code</TableCell>
                    <TableCell className={classes.th} align="right">Rate</TableCell>
                    <TableCell className={classes.th} align="right">Trend</TableCell>
                </TableRow>
            </TableHead>
            <TableBody>
                {Object.keys(config).filter(value => {
                    if (filter === '') {
                        return true;
                    }

                    const currencyPairCode = config[value][0].code + config[value][1].code;
                    const currencyPairName = config[value][0].name + config[value][1].name;

                    return matchFilter(currencyPairCode, filter) || matchFilter(currencyPairName, filter);
                }).map(key => (
                    <TableRow key={key}>
                        <TableCell>{config[key][0].name} / {config[key][1].name}</TableCell>
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