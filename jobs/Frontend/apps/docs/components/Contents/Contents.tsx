/*
 |-----------------------------------------------------------------------------
 | components/layouts/Contents/Content.tsx
 | v1.0.0
 |-----------------------------------------------------------------------------
 */

import type { ReactElement } from 'react';

import { Link } from '@repo/assets/icons';
import { twJoin } from 'tailwind-merge';

import type { IContentsProps, TItem } from './types';

const Contents = ({ items }: IContentsProps): ReactElement => {
	return (
		<>
			{items &&
				items.map((item: TItem, index: number) => (
					<div
						key={index}
						className={twJoin(
							'border-b',
							'border-b-storybook-grey',
							'border-dashed',
							'flex',
							'items-center',
							'justify-between',
							'px-8',
							'py-4',
							'w-full',
						)}
					>
						{item.label}

						<a href={item.path}>
							<Link className="fill-storybook-blue size-4" />{' '}
							<span className="sr-only">Go to</span>
						</a>
					</div>
				))}
		</>
	);
};

export default Contents;
