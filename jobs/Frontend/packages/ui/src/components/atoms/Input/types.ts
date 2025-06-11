/*
 |-----------------------------------------------------------------------------
 | src/components/atoms/Input/types.ts
 | v1.0.0
 |-----------------------------------------------------------------------------
 */

import type { ChangeEventHandler, FocusEventHandler } from 'react';

interface IInputProps {
	autocomplete?: string;
	classNames?: string;
	defaultValue?: string | number;
	id: string;
	isDisabled?: boolean;
	isInvalid?: boolean;
	isOptional?: boolean;
	isReadonly?: boolean;
	name: string;
	onBlur?: FocusEventHandler<HTMLInputElement> | undefined;
	onChange?: ChangeEventHandler<HTMLInputElement> | undefined;
	placeholder?: string;
	type?:
		| 'date'
		| 'email'
		| 'number'
		| 'password'
		| 'tel'
		| 'text'
		| 'time'
		| 'url';
}

export type { IInputProps };
