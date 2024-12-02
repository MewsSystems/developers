import { ReactNode, useEffect, useState } from 'react';
import { LoaderWrapper, LoadingIndicator } from './Loader.styled';
import { VisuallyHidden } from './A11y';

export function Loader({children}: { children?: ReactNode }) {
    const [isShown, setIsShown] = useState(false);

    // flicker prevention
    useEffect(() => {
        const timer = setTimeout(() => {
            setIsShown(true);
        }, 200);

        return () => { clearTimeout(timer); };
    });

    return (
        <LoaderWrapper style={{display: isShown ? 'inline-block' : 'none'}}>
            {
                children
                    ? children
                    : <VisuallyHidden>Loading</VisuallyHidden>
            }
            <LoadingIndicator aria-hidden="true"/>
        </LoaderWrapper>
    );
}