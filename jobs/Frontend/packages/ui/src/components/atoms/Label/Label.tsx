/*
 |-----------------------------------------------------------------------------
 | src/components/atoms/Label/Label.tsx
 | v1.0.0
 |-----------------------------------------------------------------------------
 */

import type { ReactElement } from 'react';

import { twMerge } from 'tailwind-merge';

import { sizes, weights } from './constants';
import type { ILabelProps } from './types';

const Label = ({
	classNames,
	id,
	isDisabled,
	isHidden,
	size = 'md',
	text,
	weight = 'normal',
}: ILabelProps): ReactElement => {
	return (
		<label
			className={twMerge(
				'cursor-pointer',
				'text-grey-800',
				sizes[size],
				weights[weight],
				isDisabled && 'pointer-events-none',
				isHidden && 'sr-only',
				classNames,
			)}
			data-testid="label"
			htmlFor={id}
		>
			{text}
		</label>
	);
};

export default Label;
