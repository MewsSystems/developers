import React from 'react';
import Title from './components/Title';

interface Props {
    className?: string,
}

export default function Header({ className }: Props) {
    return (
        <header className={className}>
            <Title />
        </header>
    );
}
