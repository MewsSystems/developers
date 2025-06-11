/*
 |-----------------------------------------------------------------------------
 | src/components/atoms/Close/Close.tsx
 | v1.0.0
 |-----------------------------------------------------------------------------
 */

import type { ReactElement } from 'react';

import { twMerge } from 'tailwind-merge';

import { colours, sizes, thicknesses } from './constants';
import type { ICloseProps } from './types';

const Close = ({
	classNames,
	colour = 'grey-800',
	hover = 'grey-800',
	isDisabled,
	label,
	onClick,
	size = 'md',
	thickness = 'normal',
}: ICloseProps): ReactElement => {
	return (
		<button
			className={twMerge(
				'cursor-pointer',
				'after:-rotate-45',
				'after:absolute',
				'after:left-1/2',
				'after:top-1/2',
				'after:transition-colors',
				'after:-translate-x-1/2',
				'after:-translate-y-1/2',
				'before:rotate-45',
				'before:absolute',
				'before:left-1/2',
				'before:top-1/2',
				'before:transition-colors',
				'before:-translate-x-1/2',
				'before:-translate-y-1/2',
				'relative',
				'size-11',
				colours[colour],
				sizes[size],
				thicknesses[thickness],
				classNames,
				colours.hover[hover],
				'focus',
				'active',
			)}
			data-testid="close"
			disabled={isDisabled}
			onClick={onClick}
			type="button"
		>
			<span className="sr-only">{label}</span>
		</button>
	);
};

export default Close;
