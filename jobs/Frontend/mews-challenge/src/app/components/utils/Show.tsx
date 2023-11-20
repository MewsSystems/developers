import React, { FC, ReactElement } from 'react';

const T: FC<{ when: boolean, children: ReactElement[] | ReactElement }> = ( { when, children } ) => {

    return (
        <>
            {when && (children)}
        </>
    )
}

export default T;