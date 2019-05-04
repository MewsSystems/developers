import React from 'react';
import { connect } from 'react-redux';
import PropTypes from 'prop-types';

const ErrorDisplayer = ({ fetchError }) => {
  const getContent = () => ((!fetchError) ? null : <small className="text-danger">Error fetching data, retry in a second.</small>);

  return (
    <div className="p-3">{getContent()}</div>
  );
};

ErrorDisplayer.propTypes = {
  fetchError: PropTypes.string,
};

ErrorDisplayer.defaultProps = {
  fetchError: null,
};

const mapStateToProps = state => ({ fetchError: state.rates.fetchError });

export default connect(mapStateToProps)(ErrorDisplayer);
