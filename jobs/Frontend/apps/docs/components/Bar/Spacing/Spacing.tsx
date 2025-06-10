/*
 |-----------------------------------------------------------------------------
 | components/Bar/Spacing/Spacing.tsx
 | v1.0.0
 |-----------------------------------------------------------------------------
 */

import type { ReactElement } from 'react';

import type { IBarProps } from '../types';

const Sizing = ({ width }: Pick<IBarProps, 'width'>): ReactElement => {
	return (
		<div className="flex items-center gap-2">
			<div
				className="bg-storybook-blue h-5"
				style={{
					width: `${width}px`,
				}}
			></div>
			{width}px
		</div>
	);
};

export default Sizing;
