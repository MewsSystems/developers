import React from 'react';
import PropTypes from 'prop-types';
import { Modal, ModalBody } from 'reactstrap';
import Spinner from 'react-spinkit';

const LoadingModal = ({ showLoadingModal }) =>
    <Modal isOpen={showLoadingModal}>
        <ModalBody>
            Fetching currency rates from server... <Spinner name="three-bounce" fadeIn="half" className="float-right" />
        </ModalBody>
    </Modal>;

LoadingModal.propTypes = {
    showLoadingModal: PropTypes.bool,
};

export default LoadingModal;
