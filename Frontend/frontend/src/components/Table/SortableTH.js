import React, { Component, } from 'react';
import {
  func, string, oneOfType, number, shape,
} from 'prop-types';

import { SORT_ASC, SORT_DES, SORT_UNSET, } from '../../globals';
import SortAsc from '../../atoms/Icons/SortAsc';
import SortDes from '../../atoms/Icons/SortDes';
import SortUnset from '../../atoms/Icons/SortUnset';


const SORT_ICONS = {
  [SORT_ASC]: <SortAsc />,
  [SORT_DES]: <SortDes />,
  [SORT_UNSET]: <SortUnset />,
};


class SortableTH extends Component {
  handleChangeSort = () => {
    const {
      id,
      value: { name, order, },
      onChangeSort,
    } = this.props;

    // init order
    let newValue = SORT_ASC;
    // first click
    if (id !== name) newValue = SORT_ASC;
    // repeated click
    else if (order === SORT_ASC) newValue = SORT_DES;

    onChangeSort(id, newValue);
  }


  renderSortIcon = () => {
    const {
      id,
      value: { name, order, },
    } = this.props;

    if (name !== id) return SORT_ICONS[SORT_UNSET];

    if (Object.prototype.hasOwnProperty.call(SORT_ICONS, order)) return SORT_ICONS[order];
    return SORT_ICONS[SORT_UNSET];
  }


  render() {
    const {
      id,
      label,
      value,
      className,
      onChangeSort,
      ...rest
    } = this.props;

    return (
      <th
        className={`table--th-sortable ${className}`}
        onClick={this.handleChangeSort}
        {...rest}
      >
        <div className="table--th-sortable-content">
          <span className="table--th-sortable-label">{label}</span>
          <span className="table--th-sortable-icon">{this.renderSortIcon()}</span>
        </div>
      </th>
    );
  }
}


SortableTH.propTypes = {
  id: oneOfType([ string, number, ]).isRequired,
  label: string.isRequired,
  value: shape({
    name: oneOfType([ string, number, ]),
    order: string,
  }).isRequired,
  className: string,
  onChangeSort: func.isRequired,
};

SortableTH.defaultProps = {
  className: '',
};


export default SortableTH;
