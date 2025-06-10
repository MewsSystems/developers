/*
 |-----------------------------------------------------------------------------
 | src/components/atoms/Title/constants.tsx
 | v1.0.0
 |-----------------------------------------------------------------------------
 */

const colours: {
	'grey-200': string;
	'grey-800': string;
} = {
	'grey-200': 'text-grey-200',
	'grey-800': 'text-grey-800',
};

const heights: {
	none: string;
	tight: string;
	snug: string;
	normal: string;
} = {
	none: 'leading-none',
	tight: 'leading-tight',
	snug: 'leading-snug',
	normal: 'leading-normal',
};

const lines: string[] = [
	'line-clamp-none',
	'line-clamp-1',
	'line-clamp-2',
	'line-clamp-3',
	'line-clamp-4',
	'line-clamp-5',
	'line-clamp-6',
];

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

const transforms: {
	capitalize: string;
	lowercase: string;
	uppercase: string;
} = {
	capitalize: 'capitalize',
	lowercase: 'lowercase',
	uppercase: 'uppercase',
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
	black: 'font-black',
};

export { colours, heights, lines, sizes, transforms, weights };
