/*
 |-----------------------------------------------------------------------------
 | components/layouts/Grid/Grid.tsx
 | v1.0.0
 |-----------------------------------------------------------------------------
 */

import type { ReactElement } from 'react';

import { twJoin } from 'tailwind-merge';

import type { IGridProps } from './types';

const Grid = ({ layout }: IGridProps): ReactElement => {
	return (
		<div className="relative">
			<div className={twJoin('grid', layout)}>
				<div className="bg-storybook-blue">&nbsp;</div>
				<div className="bg-storybook-pink">&nbsp;</div>
			</div>
			<div
				className={twJoin(
					'absolute',
					'grid',
					'grid-cols-12',
					'left-0',
					'text-black',
					'text-center',
					'top-0',
					'w-full',
				)}
			>
				<div>01</div>
				<div>02</div>
				<div>03</div>
				<div>04</div>
				<div>05</div>
				<div>06</div>
				<div>07</div>
				<div>08</div>
				<div>09</div>
				<div>10</div>
				<div>11</div>
				<div>12</div>
			</div>
		</div>
	);
};

export default Grid;
