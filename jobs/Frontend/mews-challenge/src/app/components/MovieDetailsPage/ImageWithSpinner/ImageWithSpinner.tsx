import React, { useState } from 'react';
import { Image } from '../../../elements/Image';
import Show from '../../utils/Show';
import Spinner from '../../common/Spinner/Spinner';
import { Color } from '../../../types';

function ImageWithSpinner(props: any) {

    const [loading, setLoading] = useState(true);

    const loadingStyle = {
        display: 'none'
    }

    const imageStyle = loading ? loadingStyle : {};

    return (
        <>
            <Show when={loading}>
                <Spinner />
            </Show>
            <Image style={imageStyle} onLoad={() => setLoading(false)} {...props}></Image>
        </>
    );
}

export default ImageWithSpinner;
