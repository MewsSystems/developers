import React from 'react';
import { trunc } from 'Utils';

const renderTableData = data => {
  return Object.entries(data).map(([code, obj]) => (
    <tr key={code}>
      <td>{trunc(code) }</td>
      <td>{obj.codeFrom}</td>
      <td>{obj.codeTo}</td>
      <td>{obj.rate || 'n/a'}</td>
      <td>{obj.trend || 'n/a'}</td>
    </tr>
  ))
};

const TableBody = ({ data }) => (
  <tbody>
    {renderTableData(data)}
  </tbody>
)

export default TableBody;
