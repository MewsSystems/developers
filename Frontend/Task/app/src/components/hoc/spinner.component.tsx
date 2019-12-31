import React from 'react';
import Spinner from 'react-spinkit'

const WithSpinner = WrappedComponent => ({ isLoading, ...otherProps }) => {
  return isLoading ? <Spinner /> : <WrappedComponent {...otherProps} />
};

export default WithSpinner;