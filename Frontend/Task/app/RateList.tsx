import React from 'react';
import { Currency } from './Currency';
import { Trend } from './Trend';
import styled from 'styled-components';

const Table = styled.table`
  width: 100%;
  border-collapse: collapse;
  table-layout: fixed;
  &,
  & tr,
  & td {
    border: 1px solid #000;
  }
  & th,
  & td {
    padding: 5px;
  }
  & tbody {
    & tr:nth-child(even) {
      background: #eee;
    }
    & tr:hover {
      background: #ddd;
    }
  }
`;

type Props = {
  pairs: ReadonlyArray<{
    id: string;
    currencies: [Currency, Currency];
    rate: number | null;
    trend: Trend;
  }>;
};

export const RateList = ({ pairs }: Props) => (
  <Table>
    <thead>
      <tr>
        <th>Pair</th>
        <th align="center">Rate</th>
        <th align="right">Trend</th>
      </tr>
    </thead>
    <tbody>
      {pairs.map(pair => (
        <tr key={pair.id}>
          <td>{`${pair.currencies[0].code}-${pair.currencies[1].code}`}</td>
          <td align="center">{pair.rate === null ? '???' : pair.rate}</td>
          <td align="right">{pair.rate === null ? '???' : pair.trend}</td>
        </tr>
      ))}
      {pairs.length === 0 && (
        <tr>
          <td colSpan={3}>No pairs selected</td>
        </tr>
      )}
    </tbody>
  </Table>
);
