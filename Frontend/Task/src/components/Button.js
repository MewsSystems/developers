import React, { Component } from 'react';

class Button extends Component {
  render() {
    const { icon, loading, disabled, children, onClick } = this.props;
    return (
      <button className="btn-success" onClick={onClick} disabled={loading || disabled}>
        {loading ?
          <i className={`fas fa-spinner`} />
          :
          icon ? <i className={`${icon} mr-10`} />
            :
            ''
        }
        {children}
      </button>
    );
  };
};

export default Button;