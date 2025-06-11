/*
 |-----------------------------------------------------------------------------
 | src/components/atoms/Close/types.ts
 | v1.0.0
 |-----------------------------------------------------------------------------
 */

import type { MouseEventHandler } from 'react';

interface ICloseProps {
	classNames?: string;
	colour?: 'grey-200' | 'grey-800';
	hover?: 'grey-200' | 'grey-800';
	isDisabled?: boolean;
	label: string;
	onClick?: MouseEventHandler<HTMLButtonElement> | undefined;
	size?: 'xs' | 'sm' | 'md' | 'lg' | 'xl';
	thickness?: 'thin' | 'normal' | 'thick';
}

export type { ICloseProps };
