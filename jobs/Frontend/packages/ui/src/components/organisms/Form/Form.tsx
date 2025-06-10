/*
 |-----------------------------------------------------------------------------
 | src/components/organisms/Form/Form.tsx
 | v1.0.0
 |-----------------------------------------------------------------------------
 */

import type { ReactElement } from 'react';

import { twMerge } from 'tailwind-merge';

import type { IFormProps } from './types';

const Form = ({
	autocomplete = 'off',
	children,
	classNames,
	isUpload,
	onChange,
	onSubmit,
}: IFormProps): ReactElement => {
	return (
		<form
			className={twMerge(classNames)}
			acceptCharset="UTF-8"
			autoComplete={autocomplete}
			data-testid="form"
			encType={isUpload ? 'multipart/form-data' : undefined}
			method="post"
			noValidate
			onChange={onChange}
			onSubmit={onSubmit}
		>
			{children}
		</form>
	);
};

export default Form;
