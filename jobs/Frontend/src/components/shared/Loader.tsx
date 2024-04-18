import { useState, useEffect } from 'react';
import { LoaderWrapper, LoadingIndicator } from './Loader.styled';

export function Loader() {
    const [isShown, setIsShown] = useState(false);

    // flicker prevention
    useEffect(() => {
        const timer = setTimeout(() => {
            setIsShown(true);
        }, 200);

        return () => clearTimeout(timer);
    });

    return (
        <LoaderWrapper style={{ display: isShown ? 'block' : 'none' }}>
            <LoadingIndicator aria-hidden="true" />            
            <p>Loading</p>
        </LoaderWrapper>
    );
};