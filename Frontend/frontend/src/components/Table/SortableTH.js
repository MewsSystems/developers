import React, { Component, } from 'react';
import {
  func, string, oneOfType, number, shape,
} from 'prop-types';

import { SORT_ASC, SORT_DES, SORT_UNSET, } from '../../globals';


const SORT_ICONS = {
  [SORT_ASC]: <span>ASC</span>,
  [SORT_DES]: <span>DES</span>,
  [SORT_UNSET]: <span>UNS</span>,
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
      label,
    } = this.props;

    return (
      <th
        onClick={this.handleChangeSort}
      >
        <span>{label}</span>
        {this.renderSortIcon()}
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
  onChangeSort: func.isRequired,
};


export default SortableTH;
