/*
 |-----------------------------------------------------------------------------
 | components/Bar/types.ts
 | v1.0.0
 |-----------------------------------------------------------------------------
 */

interface IBarProps {
	height?: number;
	label?: string;
	size?: 'none' | 'lg' | '2xl' | 'full';
	variant: 'borderRadius' | 'borderWidth' | 'sizing' | 'spacing';
	width?: number;
}

export type { IBarProps };
