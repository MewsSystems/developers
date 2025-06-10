/*
 |-----------------------------------------------------------------------------
 | src/components/atoms/Title/Title.tsx
 | v1.0.0
 |-----------------------------------------------------------------------------
 */

import type { ElementType, ReactElement } from 'react';

import { twMerge } from 'tailwind-merge';

import {
	colours,
	heights,
	lines,
	sizes,
	transforms,
	weights,
} from './constants';
import type { ITitleProps } from './types';

const Title = ({
	children,
	clamp,
	classNames,
	colour = 'grey-800',
	isSrOnly,
	leading,
	level,
	schema,
	size = 'md',
	text,
	transform,
	weight = 'bold',
}: ITitleProps): ReactElement => {
	// console.log('Debug atom Title:', {
	// 	// clamp: clamp,
	// 	// classNames: classNames,
	// 	// colour: colour,
	// 	// isSrOnly: isSrOnly,
	// 	// leading: leading,
	// 	// level: level,
	// 	// schema: schema,
	// 	// size: size,
	// 	// text: text,
	// 	// transform: transform,
	// 	// weight: weight,
	// });

	const Tag: ElementType = `h${level}`;

	return (
		<Tag
			className={twMerge(
				colours[colour],
				sizes[size],
				weights[weight],
				clamp && lines[clamp],
				isSrOnly && 'sr-only',
				leading && heights[leading],
				transform && transforms[transform],
				classNames,
			)}
			data-testid="title"
			itemProp={schema}
		>
			{children || text}
		</Tag>
	);
};

export default Title;
