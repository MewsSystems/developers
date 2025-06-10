/*
 |-----------------------------------------------------------------------------
 | src/components/layouts/Container/Container.tsx
 | v1.0.0
 |-----------------------------------------------------------------------------
 */

import type { ElementType, ReactElement } from 'react';

import { twMerge } from 'tailwind-merge';

import type { IContainerProps } from './types';
import { widths } from './constants';

const Container = ({
	children,
	classNames,
	isCentered,
	isFullWidth,
	tag = 'div',
	width = '2xl',
}: IContainerProps): ReactElement => {
	const Tag: ElementType = tag;

	return (
		<Tag
			className={twMerge(
				'mx-auto',
				'w-full',
				widths[width],
				isCentered && 'flex justify-center text-center',
				isFullWidth ? 'max-w-none px-0' : 'px-md',
				classNames,
			)}
		>
			{children}
		</Tag>
	);
};

export default Container;
