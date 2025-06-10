/*
 |-----------------------------------------------------------------------------
 | src/components/molecules/Alert/Alert.tsx
 | v1.0.0
 |-----------------------------------------------------------------------------
 */

'use client';

import type { ReactElement } from 'react';

import { CheckCircle, Error, Info, Warning } from '@repo/assets/icons';
import Icon from '@ui/components/helpers/Icon';
import dynamic from 'next/dynamic';
import { twMerge } from 'tailwind-merge';

import type { IAlertProps, TAlertVariants } from './types';
import { useAlertState } from './hooks';

const Close = dynamic(() => import('@ui/components/atoms/Close'));

const Alert = ({
	classNames,
	text,
	variant = 'info',
}: IAlertProps): ReactElement | null => {
	const { setVisible, visible } = useAlertState();

	const variants: TAlertVariants = {
		error: {
			classes: 'bg-error-50 border-error-500 text-error-500',
			icon: 'Error',
		},
		info: {
			classes: 'bg-info-50 border-info-500 text-info-500',
			icon: 'Info',
		},
		success: {
			classes: 'bg-success-50 border-success-500 text-success-500',
			icon: 'CheckCircle',
		},
		warning: {
			classes: 'bg-warning-50 border-warning-500 text-warning-500',
			icon: 'Warning',
		},
	};

	if (!visible) {
		return null;
	}

	return (
		<div
			className={twMerge(
				'border-l-solid',
				'flex',
				'gap-x-xs',
				'items-center',
				'border-l-8',
				'pl-xs',
				variants[variant].classes,
				classNames,
			)}
			data-testid="alert"
			role="alert"
		>
			<Icon
				classNames="size-sm fill-current"
				icons={{
					CheckCircle,
					Error,
					Info,
					Warning,
				}}
				name={variants[variant].icon}
			/>

			<p className="text-sm font-normal text-black">{text}</p>

			<Close
				classNames="ml-auto"
				label="Dismiss alert"
				onClick={() => setVisible(false)}
				size="sm"
			/>
		</div>
	);
};

export default Alert;
