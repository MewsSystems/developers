/*
 |-----------------------------------------------------------------------------
 | components/Bar/BorderRadius/BorderRadius.tsx
 | v1.0.0
 |-----------------------------------------------------------------------------
 */

import type { ReactElement } from 'react';

import { twJoin } from 'tailwind-merge';

import type { IBarProps } from '../types';

const BorderRadius = ({
	label,
	size,
}: Pick<IBarProps, 'label' | 'size'>): ReactElement => {
	const sizes = {
		none: 'rounded-none',
		lg: 'rounded-lg',
		'2xl': 'rounded-2xl',
		full: 'rounded-full',
	};

	return (
		<div className="flex items-center gap-2">
			<div
				className={twJoin(
					'bg-storybook-blue',
					'h-10',
					'w-10',
					sizes[size as keyof typeof sizes] || 'rounded',
				)}
			></div>
			{label}
		</div>
	);
};

export default BorderRadius;
