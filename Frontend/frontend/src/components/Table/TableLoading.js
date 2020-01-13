import React from 'react';
import { number, } from 'prop-types';


const TableLoading = ({ colSpan, }) => (
  <tbody>
    <tr>
      <td
        className="table--loading-td"
        colSpan={colSpan}
      >
        <div className="table--loading-content">
          <span>Loading...</span>
        </div>
      </td>
    </tr>
  </tbody>
);


TableLoading.propTypes = {
  colSpan: number.isRequired,
};


export default TableLoading;
