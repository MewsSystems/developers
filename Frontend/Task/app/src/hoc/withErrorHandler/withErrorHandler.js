import React from 'react'

import useHttpHelper from "../../hooks/httpErrorHandler";

import Modal from '../../components/UI/Modal/Modal'

const withErrorHandler = (WrappedComponent, axios) => {
    return props => {

        const [ error, clearError ] = useHttpHelper(axios)

        return(
            <>
                <Modal
                    show={error}
                    modalClosed={clearError}
                >
                    {error ? error.message : null}
                </Modal>
                <WrappedComponent {...props}/>
            </>
        )

    }
}

export default  withErrorHandler
