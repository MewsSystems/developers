/*
 |-----------------------------------------------------------------------------
 | src/components/atoms/Error/Error.tsx
 | v1.0.0
 |-----------------------------------------------------------------------------
 */

import type { ReactElement } from 'react';

import { twMerge } from 'tailwind-merge';

import type { IErrorProps } from './types';

const Error = ({ classNames, text }: IErrorProps): ReactElement => {
	return (
		<p
			className={twMerge(
				'font-normal',
				'text-error-500',
				'text-md',
				classNames,
			)}
			aria-live="assertive"
			data-testid="error"
		>
			{text}
		</p>
	);
};

export default Error;
