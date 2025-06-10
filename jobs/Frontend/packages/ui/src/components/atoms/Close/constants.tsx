/*
 |-----------------------------------------------------------------------------
 | src/components/atoms/Close/constants.ts
 | v1.0.0
 |-----------------------------------------------------------------------------
 */

const colours: {
	'grey-200': string;
	'grey-800': string;
	hover: {
		'grey-200': string;
		'grey-800': string;
	};
} = {
	'grey-200': 'after:bg-grey-200 before:bg-grey-200',
	'grey-800': 'after:bg-grey-800 before:bg-grey-800',
	hover: {
		'grey-200': 'hover:after:bg-grey-200 hover:before:bg-grey-200',
		'grey-800': 'hover:after:bg-grey-800 hover:before:bg-grey-800',
	},
};

const sizes: {
	xs: string;
	sm: string;
	md: string;
	lg: string;
	xl: string;
} = {
	xs: 'after:h-xs before:h-xs',
	sm: 'after:h-sm before:h-sm',
	md: 'after:h-md before:h-md',
	lg: 'after:h-lg before:h-lg',
	xl: 'after:h-xl before:h-xl',
};

const thicknesses: {
	thin: string;
	normal: string;
	thick: string;
} = {
	thin: 'after:w-px before:w-px',
	normal: 'after:w-0.5 before:w-0.5',
	thick: 'after:w-1 before:w-1',
};

export { colours, sizes, thicknesses };
