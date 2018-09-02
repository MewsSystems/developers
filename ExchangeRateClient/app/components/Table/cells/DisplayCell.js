import React from 'react';
import PropTypes from 'prop-types';

const DisplayCell = ({ cellData }) =>
    <td>{cellData}</td>;

DisplayCell.propTypes = {
    cellData: PropTypes.any,
};

export default DisplayCell;
