import React from 'react';
import { connect } from 'react-redux';
import { Wrapper } from './Error.styles';
import Loader from '../Loader/Loader';

const Error = (props) => {
    return (
      <Wrapper error={props.error}>
          <div className="modal">
              <p>oops something went wrong</p>
              <p>Trying to reconnect ....</p>
              <Loader />
          </div>
      </Wrapper>
    );
}

const mapStateToProps = state => {
  const error = state.error;
  return {
    error : error
  };
}

export default connect(mapStateToProps)(Error);
