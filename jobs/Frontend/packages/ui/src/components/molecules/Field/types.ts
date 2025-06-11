/*
 |-----------------------------------------------------------------------------
 | src/components/molecules/Field/types.ts
 |-----------------------------------------------------------------------------
 */

import type { ReactNode } from 'react';

interface IFieldProps {
	children: ReactNode;
	classNames?: string;
	error?: string;
	hasHiddenLabel?: boolean;
	hint?: string;
	id: string;
	label: string;
}

export type { IFieldProps };
