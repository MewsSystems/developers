/*
 |-----------------------------------------------------------------------------
 | src/components/atoms/Label/constants.tsx
 | v1.0.0
 |-----------------------------------------------------------------------------
 */

const sizes: {
	xs: string;
	sm: string;
	md: string;
	lg: string;
	xl: string;
	'2xl': string;
	'3xl': string;
	'4xl': string;
	'5xl': string;
} = {
	xs: 'text-xs',
	sm: 'text-sm',
	md: 'text-md',
	lg: 'text-lg',
	xl: 'text-xl',
	'2xl': 'text-2xl',
	'3xl': 'text-3xl',
	'4xl': 'text-4xl',
	'5xl': 'text-5xl',
};

const weights: {
	thin: string;
	extralight: string;
	light: string;
	normal: string;
	medium: string;
	semibold: string;
	bold: string;
	extrabold: string;
	black: string;
} = {
	thin: 'font-thin',
	extralight: 'font-extralight',
	light: 'font-light',
	normal: 'font-normal',
	medium: 'font-medium',
	semibold: 'font-semibold',
	bold: 'font-bold',
	extrabold: 'font-extrabold',
	black: 'font-bold',
};

export { sizes, weights };
