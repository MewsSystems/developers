import React from 'react';
import { number, func, } from 'prop-types';

import Button from '../../atoms/Button/Button';


const TableError = ({ colSpan, onRefresh, }) => (
  <tbody>
    <tr>
      <td
        className="table--error-td"
        colSpan={colSpan}
      >
        <div className="table--error-content">
          <div>Something Happened!</div>
          {onRefresh && (
            <div>
              <Button
                type="button"
                onClick={onRefresh}
              >
                Refresh
              </Button>
            </div>
          )}
        </div>
      </td>
    </tr>
  </tbody>
);


TableError.propTypes = {
  colSpan: number.isRequired,
  onRefresh: func,
};

TableError.defaultProps = {
  onRefresh: undefined,
};


export default TableError;
