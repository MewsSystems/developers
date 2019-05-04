import React from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';

const LastFetched = ({ date = null }) => {
  const getStringDate = () => ((date) ? (new Date(date)).toLocaleTimeString() : '');
  const getReturn = () => (
    <div>
      <i style={{ marginRight: '10px' }}>Last update:</i>
      {getStringDate(date)}
    </div>
  );
  return (
    <div className="p-3">
      {(date) ? getReturn() : null}
    </div>
  );
};

LastFetched.propTypes = {
  date: PropTypes.number,
};

LastFetched.defaultProps = {
  date: null,
};

const mapStateToProps = state => ({ date: state.rates.lastFetched });

export default connect(mapStateToProps)(LastFetched);
