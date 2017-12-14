import React from 'react';

import './RatesTable.css';

function renderTrend(trend) {
    if (trend === null) {
        return '?';
    }

    if (trend === 1) {
        return '↗';
    }

    if (trend === -1) {
        return '↘';
    }

    return '→';
}

export default function RatesTable({data}) {
    if (data === null) {
        return <p>Loading...</p>;
    }

    return <table className="RatesTable">
        <thead>
            <tr>
                <th>From</th>
                <th>To</th>
                <th>Rate</th>
                <th>Trend</th>
            </tr>
        </thead>
        <tbody>
            {data.map(row => <tr key={row.id}>
                <td>
                    <span title={row.fromCurrency.name}>{row.fromCurrency.code}</span>
                </td>
                <td>
                    <span title={row.toCurrency.name}>{row.toCurrency.code}</span>
                </td>
                <td>
                    {row.rate}
                </td>
                <td>
                    {renderTrend(row.trend)}
                </td>
            </tr>)}
        </tbody>
    </table>;
}
