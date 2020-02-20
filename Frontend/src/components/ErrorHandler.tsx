import React from 'react';

interface Props {
    tryItAgain: Function,
}

export default function ErrorHandler(props: Props) {
    const tryItAgain = (event: React.MouseEvent<HTMLAnchorElement>) => {
        event.preventDefault();
        props.tryItAgain();
    };

    return (
        <span>
            Something has failed.
            <a onClick={tryItAgain} href={''}>Try it again.</a>
        </span>
    );
}
