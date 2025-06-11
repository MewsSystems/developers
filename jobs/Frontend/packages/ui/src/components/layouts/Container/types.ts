/*
 |-----------------------------------------------------------------------------
 | src/components/layouts/Container/types.ts
 | v1.0.0
 |-----------------------------------------------------------------------------
 */

import type { ReactNode } from 'react';

interface IContainerProps {
	children: ReactNode;
	classNames?: string;
	isCentered?: boolean;
	isFullWidth?: boolean;
	tag?: 'div' | 'footer' | 'header';
	width?: 'xs' | 'sm' | 'md' | 'lg' | 'xl' | '2xl' | 'body';
}

export type { IContainerProps };
