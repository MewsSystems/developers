/*
 |-----------------------------------------------------------------------------
 | components/Bar/Sizing/Sizing.tsx
 | v1.0.0
 |-----------------------------------------------------------------------------
 */

import type { ReactElement } from 'react';

import type { IBarProps } from '../types';

const Sizing = ({
	height,
	width,
}: Pick<IBarProps, 'height' | 'width'>): ReactElement => {
	return (
		<div className="flex items-center gap-2">
			<div
				className="bg-strapless-primary"
				style={{
					height: `${height}px`,
					width: `${width}px`,
				}}
			></div>
			{height}px
		</div>
	);
};

export default Sizing;
