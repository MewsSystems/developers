import React, { Component } from 'react';


import { AlertMessagesProps, AlertMessagesState } from './types';

import Icon from '../Icon';
import IconClose from '@icons/close';
import { AlertContainer, AlertMessage, AlertCloseButton } from './AlertMessages.styled';

export class AlertMessages extends Component<AlertMessagesProps, AlertMessagesState> {
    private timer: number;

    constructor(props: AlertMessagesProps) {
        super(props);

        this.timer = 0;
        this.state = {
            show: false
        }
    }

    static getDerivedStateFromProps(props: AlertMessagesProps, state: AlertMessagesState) {
        return {
            show: props.show
        };
    }

    componentDidUpdate(prevProps: AlertMessagesProps, prevState: AlertMessagesState) {
        window.clearTimeout(this.timer);

        this.setTimer();
    }

    componentWillUnmount() {
        window.clearTimeout(this.timer);
    }

    setTimer = () => {
        this.timer = window.setTimeout(() => {
            this.hide();
        }, 6000)
    }

    hide = () =>{
        this.setState({
            show: false
        }, () => {
            this.props.onHide && this.props.onHide();
        })
    }

    render() {
        const { message } = this.props;
        const { show } = this.state;

        return (
            <AlertContainer className={show ? 'is-visible' : undefined}>
                <AlertMessage>
                    {message}
                </AlertMessage>
                <AlertCloseButton onClick={this.hide}>
                    <Icon svgPaths={IconClose} />
                </AlertCloseButton>
            </AlertContainer>
        )
    }
}