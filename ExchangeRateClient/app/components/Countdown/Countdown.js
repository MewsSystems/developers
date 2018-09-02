import React from 'react';
import PropTypes from 'prop-types';
import { Alert } from 'reactstrap';
import { secondsToTime } from '../../utils';

class Countdown extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            time: secondsToTime(props.seconds),
            seconds: props.seconds,
        };
        this.timer = 0;
    }

    componentDidMount() {
        this.startTimer();
    }

    startTimer = () => {
        if (this.timer === 0) {
            this.timer = setInterval(this.countDown, 1000);
        }
    };

    countDown = () => {
        const seconds = this.state.seconds - 1;
        this.setState({
            time: secondsToTime(seconds),
            seconds,
        });
        if (seconds === 0) {
            clearInterval(this.timer);
            this.timer = 0;
            this.setState({
                time: secondsToTime(this.props.seconds),
                seconds: this.props.seconds,
            }, this.startTimer);
        }
    };

    render() {
        return (<Alert color="info"> Update in {this.state.time.m} minutes and {this.state.time.s} seconds</Alert>);
    }
}

Countdown.propTypes = {
    seconds: PropTypes.number,
};

export default Countdown;
