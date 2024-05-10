'use client';
import { Film } from 'lucide-react';
import Link from 'next/link'
import React from 'react'
import styled from 'styled-components'

const NavigationWrapper = styled.nav`
    width: 100%;
    padding: 12px 24px;
    background-color: #212121;
    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.5);
`;

const List = styled.ul`
    list-style: none;
    display: flex;
    align-items: center;
    gap: 24px;
`

export default function Navigation() {
    return (
        <NavigationWrapper>
            <List>
                <li><Film /></li>
                <li><Link href={"/"} > Home</Link></li>
            </List>
        </NavigationWrapper>
    )
}
