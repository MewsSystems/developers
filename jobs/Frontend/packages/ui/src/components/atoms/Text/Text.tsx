/*
 |-----------------------------------------------------------------------------
 | src/components/atoms/Text/Text.tsx
 | v1.0.0
 |-----------------------------------------------------------------------------
 */

import type { ReactElement } from 'react';

import { twMerge } from 'tailwind-merge';

import {
	colours,
	heights,
	lines,
	sizes,
	transforms,
	weights,
} from './constants';
import type { ITextProps } from './types';

const Text = ({
	children,
	clamp,
	classNames,
	colour = 'grey-800',
	isItalic,
	leading,
	schema,
	size = 'md',
	text,
	transform,
	weight = 'normal',
}: ITextProps): ReactElement => {
	return (
		<p
			className={twMerge(
				colours[colour],
				sizes[size],
				weights[weight],
				clamp && lines[clamp],
				isItalic && 'italic',
				leading && heights[leading],
				transform && transforms[transform],
				classNames,
			)}
			data-testid="text"
			itemProp={schema}
		>
			{children || text}
		</p>
	);
};

export default Text;
