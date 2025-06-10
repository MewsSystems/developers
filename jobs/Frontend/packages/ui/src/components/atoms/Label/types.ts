/*
 |-----------------------------------------------------------------------------
 | src/components/atoms/Label/types.ts
 | v1.0.0
 |-----------------------------------------------------------------------------
 */

interface ILabelProps {
	classNames?: string;
	id: string;
	isDisabled?: boolean;
	isHidden?: boolean;
	size?: 'xs' | 'sm' | 'md' | 'lg' | 'xl' | '2xl' | '3xl' | '4xl' | '5xl';
	text: string;
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

export type { ILabelProps };
