import React from 'react';
import PropTypes from 'prop-types';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { __, applySpec, assoc, dissoc, either, gte, isNil } from 'ramda';
import { isNilOrEmpty, notBoth } from 'ramda-adjunct';
import {
    Button, Form, FormFeedback, FormGroup, Input, Label, Modal, ModalBody, ModalFooter, ModalHeader, NavItem, NavLink,
} from 'reactstrap';
import { saveSettings } from '../../../actions';
import { getConfig } from '../../../selectors';
import { stringIsPositiveInteger } from '../../../validations/number';

class SettingsNavItem extends React.Component {

    state = {
        settings: null,
        showModal: false,
        errorFeedback: {},
    };

    static getDerivedStateFromProps(nextProps, prevState) {
        if (isNil(prevState.settings)) {
            return { ...prevState, settings: nextProps.config };
        }
        return null;
    }

    toggle = () => this.setState({ showModal: !this.state.showModal });

    handleChangeInterval = (event) => {
        const interval = event.target.value;
        const { errorFeedback } = this.state;
        if (either(notBoth(stringIsPositiveInteger, gte(__, 1000)), isNilOrEmpty)(interval)) {
            this.setState({ errorFeedback: assoc(
                'interval',
                'Please enter a positive integer greater or equal to 1000.',
                errorFeedback
            ) });
        } else {
            this.setState({
                settings: { ...this.state.settings, interval: parseInt(interval, 10) },
                errorFeedback: dissoc('interval', errorFeedback),
            });
        }
    };

    handleSaveSettings = () => {
        const { settings, errorFeedback } = this.state;
        if (isNilOrEmpty(errorFeedback)) {
            this.props.saveSettings(settings);
            this.toggle();
        }
    };

    render() {
        const { errorFeedback } = this.state;
        const { endpoint, interval } = this.props.config;
        return (
            <React.Fragment>
                <NavItem>
                    <NavLink
                        href="#"
                        onClick={this.toggle} // eslint-disable-line react/jsx-handler-names
                    >
                        <i className="fas fa-cog fa-lg" />
                    </NavLink>
                </NavItem>
                <Modal isOpen={this.state.showModal} toggle={this.toggle}>
                    <ModalHeader toggle={this.toggle}>Settings</ModalHeader>
                    <ModalBody>
                        <Form>
                            <FormGroup>
                                <Label for="settingsEndpoint">Base endpoint</Label>
                                <Input value={endpoint} id="settingsEndpoint" disabled />
                            </FormGroup>
                            <FormGroup>
                                <Label for="settingsInterval">Fetching interval (ms)</Label>
                                <Input
                                    id="settingsInterval"
                                    placeholder={interval}
                                    invalid={!isNilOrEmpty(errorFeedback.interval)}
                                    onChange={this.handleChangeInterval}
                                />
                                { !isNilOrEmpty(errorFeedback.interval) &&
                                    <FormFeedback>{errorFeedback.interval}</FormFeedback>
                                }
                            </FormGroup>
                        </Form>
                    </ModalBody>
                    <ModalFooter>
                        <Button
                            color="primary"
                            onClick={this.handleSaveSettings}
                            disabled={!isNilOrEmpty(errorFeedback)}
                        >
                            Save
                        </Button>
                    </ModalFooter>
                </Modal>
            </React.Fragment>
        );
    }
}

SettingsNavItem.propTypes = {
    config: PropTypes.any,
    saveSettings: PropTypes.func,
};

const mapStateToProps = applySpec({
    config: getConfig,
});

const mapDispatchToProps = (dispatch) => bindActionCreators({
    saveSettings,
}, dispatch);

export default connect(mapStateToProps, mapDispatchToProps)(SettingsNavItem);
