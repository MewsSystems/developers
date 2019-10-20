import React from 'react'

import { Modal } from './styled__components'
import Backdrop from '../Backdrop/Backdrop'

const ModalWin = props => {

    return (
        <>
            <Backdrop show={props.show} clicked={props.modalClosed}/>
            <Modal
                style={{
                    transform: props.show ? 'translateY(0)' : 'translateY(-100vh)',
                    opacity: props.show ? '1' : '0'
                }}
            >
                {props.children}
            </Modal>
        </>
    )

}

export default React.memo(ModalWin, (prevProps, nextProps) =>
    nextProps.show === prevProps.show &&
    nextProps.children === prevProps.children
)