import React, { Component } from 'react';
let isUnmount = false;

class Timer extends Component {
  state = { sec: this.props.sec || 5 };
  componentDidMount() {
    this.tick();
  };

  componentWillUnmount() {
    isUnmount = true;
  };

  tick = () => {
    const _this = this;
    if (this.state.sec > 1) setTimeout(() => {
      !isUnmount && _this.setState({ sec: _this.state.sec - 1 }, () => _this.tick());
    }, 1000);
  };

  render() {
    const { sec } = this.state;
    return (
      <span>{sec}</span>
    );
  };
};

export default Timer;