import React from 'react';

interface Props {
    className?: string,
}

export default function Footer({ className }: Props) {
    return (
        <footer className={className}>
            Made by
            {' '}
            <a
                href={'https://github.com/fabulator'}
                target={'_blank'}
                rel={'noopener noreferrer'}
            >
                Michal
            </a>
            . Designed in Czech Republic. Manufactured in Czech Republic.
        </footer>
    );
}
