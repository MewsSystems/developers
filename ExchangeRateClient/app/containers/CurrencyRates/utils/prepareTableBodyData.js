import React from 'react';
import { contains, filter, map } from 'ramda';
import { isNilOrEmpty } from 'ramda-adjunct';
import RowCollapseBody from '../RowCollapseBody';
import { currencyPairToString } from '../../../utils';

const prepareData = ({ toggleTracking }) => ({
    id,
    pair,
    previousRate,
    currentRate = 'Not Given',
    trend = 'Not Given',
    growthRate,
    track,
}) => ({
    id,
    rowData: [{
        cellType: 'display',
        cellData: pair ? currencyPairToString(pair) : 'No Data',
    }, {
        cellType: 'display',
        cellData: isNilOrEmpty(previousRate) ? currentRate : <div>{previousRate} → <b>{currentRate}</b></div>,
    }, {
        cellType: 'display',
        cellData: trend,
    }, {
        cellType: 'display',
        cellData: !isNilOrEmpty(previousRate) ? `${(growthRate * 100).toFixed(4)}%` : 'No Data',
    }, {
        cellType: 'checkbox',
        cellData: { defaultValue: track, onChange: () => toggleTracking(id) },
    }],
    collapseBody: <RowCollapseBody id={id} pair={currencyPairToString(pair)} />,
});

export default (actions, orderedCurrencyPairs, displayRows) => {
    const rowDatas = map(prepareData(actions), orderedCurrencyPairs);
    return filter(({ id }) => contains(id, displayRows), rowDatas);
};
