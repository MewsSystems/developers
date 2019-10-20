import React from 'react'

import { Backdrop } from "./styled__components";

const backdrop = (props) => {
    return (props.show ? <Backdrop onClick={props.clicked}></Backdrop> : null)
}

export default backdrop