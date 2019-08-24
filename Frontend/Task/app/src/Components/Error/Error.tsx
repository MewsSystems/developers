import React from 'react';
import { connect } from 'react-redux';
import { Wrapper } from './Error.styles';
import Loader from '../Loader/Loader';
import IRootState from '../../interfaces/InitialState.interface'
import IError from '../../interfaces/Error.interface'

const Error: React.SFC<any> = (props: IError) => {
  const { error } = props;
  return (
    <Wrapper error={error}>
      <div className="modal">
        <p>oops something went wrong</p>
        <p>Trying to reconnect ....</p>
        <Loader />
      </div>
    </Wrapper>
  );
}

const mapStateToProps = (state: IRootState) => {
  const error = state.error;
  return {
    error : error
  };
}

export default connect(mapStateToProps)(Error);
