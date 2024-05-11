'use client';
import { Film } from 'lucide-react';
import Link from 'next/link'
import React from 'react'
import styled from 'styled-components'
import ThemeToggle from '@/hooks/theme.util'

const NavigationWrapper = styled.nav`
    width: 100%;
    padding: 12px 24px;
    background-color: rgb(var(--navigation-background-color));
    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.5);
`;

const List = styled.ul`
    list-style: none;
    display: flex;
    align-items: center;
    justify-content: space-between;
    gap: 24px;
`

export default function Navigation() {


    return (
        <NavigationWrapper>
            <List>
                <li><Link href={"/"} replace> <Film /></Link></li>
                <li><ThemeToggle /></li>
            </List>
        </NavigationWrapper>
    )
}
