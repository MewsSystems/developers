/*
 |-----------------------------------------------------------------------------
 | components/Bar/BorderWidth/BorderWidth.tsx
 | v1.0.0
 |-----------------------------------------------------------------------------
 */

import type { ReactElement } from 'react';

import type { IBarProps } from '../types';

const BorderWidth = ({ height }: Pick<IBarProps, 'height'>): ReactElement => {
	return (
		<div className="flex items-center gap-2">
			<div
				className="bg-storybook-blue w-10"
				style={{
					height: `${height}px`,
				}}
			></div>
			{height && height > 0 ? `${height}px` : height}
		</div>
	);
};

export default BorderWidth;
