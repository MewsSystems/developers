/*
 |-----------------------------------------------------------------------------
 | src/components/atoms/Hint/Hint.tsx
 | v1.0.0
 |-----------------------------------------------------------------------------
 */

import type { ReactElement } from 'react';

import { twMerge } from 'tailwind-merge';

import type { IHintProps } from './types';

const Hint = ({ classNames, text }: IHintProps): ReactElement => {
	return (
		<p
			className={twMerge(
				'font-normal',
				'text-grey-800',
				'text-md',
				classNames,
			)}
			data-testid="hint"
		>
			{text}
		</p>
	);
};

export default Hint;
