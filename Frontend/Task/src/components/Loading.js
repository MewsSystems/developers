import React, { Component } from 'react';

class Loading extends Component {
  render() {
    const { className } = this.props;
    return (
      <div className={`loading ${className || ''}`}>
        <i className="fas fa-spinner" />
      </div>
    );
  };
};

export default Loading;