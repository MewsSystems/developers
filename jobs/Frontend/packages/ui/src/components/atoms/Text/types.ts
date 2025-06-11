/*
 |-----------------------------------------------------------------------------
 | src/components/atoms/Text/types.ts
 | v1.0.0
 |-----------------------------------------------------------------------------
 */

import type { ReactNode } from 'react';

interface ITextProps {
	children?: ReactNode;
	clamp?: 1 | 2 | 3 | 4 | 5 | 6;
	classNames?: string;
	colour?: 'grey-200' | 'grey-800';
	isItalic?: boolean;
	leading?: 'none' | 'tight' | 'snug' | 'normal';
	schema?: 'description' | 'url';
	size?: 'xs' | 'sm' | 'md' | 'lg' | 'xl' | '2xl' | '3xl' | '4xl' | '5xl';
	text?: string;
	transform?: 'capitalize' | 'lowercase' | 'uppercase';
	weight?:
		| 'thin'
		| 'extralight'
		| 'light'
		| 'normal'
		| 'medium'
		| 'semibold'
		| 'bold'
		| 'extrabold'
		| 'black';
}

export type { ITextProps };
