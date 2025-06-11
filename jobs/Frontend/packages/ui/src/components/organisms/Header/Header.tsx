/*
 |-----------------------------------------------------------------------------
 | src/components/organisms/Header/Header.tsx
 | v1.0.0
 |-----------------------------------------------------------------------------
 */

'use client';

import { type ReactElement } from 'react';

import Container from '@repo/ui/components/layouts/Container';
import Title from '@repo/ui/components/atoms/Title';
import { twMerge } from 'tailwind-merge';

import type { IHeaderProps } from './types';

const Header = ({ classNames, title }: IHeaderProps): ReactElement => {
	return (
		<header
			className={twMerge('bg-white', 'py-sm', classNames)}
			data-testid="header"
			itemScope
			itemType="https://schema.org/Header"
		>
			<Container classNames="flex items-center justify-between">
				<Title level={1}>{title}</Title>
			</Container>
		</header>
	);
};

export default Header;
