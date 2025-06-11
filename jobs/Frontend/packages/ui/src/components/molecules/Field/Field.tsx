/*
 |-----------------------------------------------------------------------------
 | src/components/molecules/Field/Field.tsx
 | v1.0.0
 |-----------------------------------------------------------------------------
 */

import type { ReactElement } from 'react';

import Label from '@ui/components/atoms/Label';
import dynamic from 'next/dynamic';
import { twMerge } from 'tailwind-merge';

import type { IFieldProps } from './types';

const Error = dynamic(() => import('@ui/components/atoms/Error'));
const Hint = dynamic(() => import('@ui/components/atoms/Hint'));

const Field = ({
	children,
	classNames,
	error,
	hasHiddenLabel,
	hint,
	id,
	label,
}: IFieldProps): ReactElement => {
	return (
		<div
			className={twMerge('flex', 'flex-col', 'gap-y-xs', classNames)}
			data-testid="field"
		>
			<Label id={id} isHidden={hasHiddenLabel} text={label} />

			{children}

			{hint && <Hint text={hint} />}

			{error && <Error text={error} />}
		</div>
	);
};

export default Field;
