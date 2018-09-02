import React from 'react';
import PropTypes from 'prop-types';
import { Alert } from 'reactstrap';

class ErrorMessage extends React.Component {

    state = {
        showErrorMessage: true,
    };

    componentDidMount() {
        this.timeoutId = setTimeout(() => this.setState({ showErrorMessage: false }), this.props.timeout);
    }

    componentWillUnmount() {
        clearTimeout(this.timeoutId);
    }

    onDismiss = () => this.setState({ showErrorMessage: false });

    render() {
        const { errorMessage } = this.props;
        return (
            <Alert color="danger" isOpen={this.state.showErrorMessage} toggle={this.onDismiss}>
                {errorMessage}
            </Alert>
        );
    }
}

ErrorMessage.propTypes = {
    errorMessage: PropTypes.string,
    timeout: PropTypes.number,
};

export default ErrorMessage;
