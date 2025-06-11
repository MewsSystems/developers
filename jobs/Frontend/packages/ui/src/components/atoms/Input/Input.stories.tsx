/*
 |-----------------------------------------------------------------------------
 | src/components/atoms/Input/Input.stories.tsx
 | v1.0.0
 |-----------------------------------------------------------------------------
 */

import type { Meta, StoryObj } from '@storybook/react';

import Input from './Input';
import { props } from './mocks';

const meta: Meta<typeof Input> = {
	title: 'Components/Atoms/Input',
	component: Input,
	args: props,
	parameters: {
		a11y: {
			element: '[data-testid="input"]',
		},
		status: {
			type: 'ga',
		},
	},
};

export default meta;

type Story = StoryObj<typeof Input>;

export const Default: Story = {};

export const Hover: Story = {
	name: ':hover',
	parameters: {
		pseudo: {
			hover: true,
		},
	},
};

export const Focus: Story = {
	name: ':focus',
	parameters: {
		pseudo: {
			focus: true,
		},
	},
};

export const Active: Story = {
	name: ':active',
	parameters: {
		pseudo: {
			active: true,
		},
	},
};

export const Disabled: Story = {
	args: {
		isDisabled: true,
	},
	name: 'Is disabled',
};

export const Invalid: Story = {
	args: {
		isInvalid: true,
	},
	name: 'Is invalid',
};

export const Placeholder: Story = {
	args: {
		placeholder: 'Lorem ipsum dolor sit amet',
	},
	name: 'With placeholder',
};
