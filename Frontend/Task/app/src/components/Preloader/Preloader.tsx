import React from 'react'
import styled from 'styled-components'
import * as skin from './Preloader.skin'
import preloader from './preloader.svg'

type PreloaderProps = {
    className: string
}

const Preloader: React.FC<PreloaderProps> = (props) => {
    const {className} = props;

    return (
        <div className={className}>
            <img src={preloader} alt="prelaoder "/>
        </div>
    )
};

export default Preloader;