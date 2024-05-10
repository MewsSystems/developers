'use client';
import Link from 'next/link'
import React from 'react'
import styled from 'styled-components'

const NavigationWrapper = styled.nav`
    width: 100%;
    padding: 12px 24px;
    background-color: aliceblue;
    color: black;
`;

const List = styled.ul`
    list-style: none;
`

export default function Navigation() {
    return (
        <NavigationWrapper>
            <List>
                <li><Link href={"/"} > Home</Link></li>
            </List>
        </NavigationWrapper>
    )
}
