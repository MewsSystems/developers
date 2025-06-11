/*
 |-----------------------------------------------------------------------------
 | src/components/atoms/Input/Input.tsx
 | v1.0.0
 |-----------------------------------------------------------------------------
 |
 | Some description...
 */

import type { ReactElement } from 'react';

import { twMerge } from 'tailwind-merge';

import type { IInputProps } from './types';

const Input = ({
	autocomplete = 'off',
	classNames,
	defaultValue,
	id,
	isDisabled,
	isInvalid,
	isOptional,
	isReadonly,
	name,
	onBlur,
	onChange,
	placeholder,
	type = 'text',
}: IInputProps): ReactElement => {
	// console.log('Debug atom Input:', {
	// 	// autocomplete: autocomplete,
	// 	// classNames: classNames,
	// 	// defaultValue: defaultValue,
	// 	// id: id,
	// 	// isDisabled: isDisabled,
	// 	// isInvalid: isInvalid,
	// 	// isOptional: isOptional,
	// 	// isReadonly: isReadonly,
	// 	// name: name,
	// 	// placeholder: placeholder,
	// 	// type: type,
	// });

	return (
		<input
			className={twMerge(
				'bg-grey-200',
				'border',
				'border-grey-800',
				'caret-grey-800',
				'font-normal',
				'leading-normal',
				'p-xs',
				'text-grey-800',
				'text-md',
				'transition-colors',
				'w-full',
				'placeholder:text-grey-800/50',
				isInvalid && 'border-error-500',
				isReadonly && 'border-grey-200 cursor-not-allowed',
				classNames,
				// 'hover:',
				'focus',
				'active',
			)}
			autoComplete={autocomplete}
			data-testid="input"
			defaultValue={defaultValue}
			disabled={isDisabled}
			id={id}
			name={name}
			onBlur={onBlur}
			onChange={onChange}
			placeholder={placeholder}
			readOnly={isReadonly}
			required={!isOptional}
			type={type}
		/>
	);
};

export default Input;
