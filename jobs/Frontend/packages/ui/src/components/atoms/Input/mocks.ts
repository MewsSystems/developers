/*
 |-----------------------------------------------------------------------------
 | src/components/atoms/Input/mocks.ts
 | v1.0.0
 |-----------------------------------------------------------------------------
 */

import type { IInputProps } from './types';

const props: IInputProps = {
	autocomplete: 'off',
	id: 'input',
	isDisabled: false,
	isInvalid: false,
	isOptional: false,
	isReadonly: false,
	name: 'Input',
	onBlur: undefined,
	onChange: undefined,
	placeholder: undefined,
	type: 'text',
};

export { props };
