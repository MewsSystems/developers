/*
 |-----------------------------------------------------------------------------
 | src/components/organisms/Form/types.ts
 | v1.0.0
 |-----------------------------------------------------------------------------
 */

import type { FormEventHandler, ReactNode } from 'react';

interface IFormProps {
	autocomplete?: string;
	children: ReactNode;
	classNames?: string;
	isUpload?: boolean;
	onChange?: FormEventHandler<HTMLFormElement> | undefined;
	onSubmit?: FormEventHandler<HTMLFormElement> | undefined;
}

export type { IFormProps };
