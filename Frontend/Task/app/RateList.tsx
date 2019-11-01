import React from 'react';
import { Currency } from './Currency';
import { Trend } from './Trend';

type Props = {
  pairs: ReadonlyArray<{
    id: string;
    currencies: [Currency, Currency];
    rate: number | null;
    trend: Trend;
  }>;
};

export const RateList = ({ pairs }: Props) => (
  <table>
    <thead>
      <tr>
        <th>Pair</th>
        <th>Rate</th>
        <th>Trend</th>
      </tr>
    </thead>
    <tbody>
      {pairs.map(pair => (
        <tr key={pair.id}>
          <td>{`${pair.currencies[0].code}-${pair.currencies[1].code}`}</td>
          <td>{pair.rate === null ? '???' : pair.rate}</td>
          <td>{pair.rate === null ? '???' : pair.trend}</td>
        </tr>
      ))}
    </tbody>
  </table>
);
