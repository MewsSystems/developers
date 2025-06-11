/*
 |-----------------------------------------------------------------------------
 | src/components/atoms/Close/Close.stories.tsx
 | v1.0.0
 |-----------------------------------------------------------------------------
 */

import type { Meta, StoryObj } from '@storybook/react';

import Close from './Close';
import { props } from './mocks';

const meta: Meta<typeof Close> = {
	title: 'Components/Atoms/Close',
	component: Close,
	argTypes: {
		colour: {
			control: {
				type: 'select',
			},
		},
		hover: {
			control: {
				type: 'select',
			},
		},
		size: {
			control: {
				type: 'select',
			},
		},
		thickness: {
			control: {
				type: 'select',
			},
		},
	},
	args: props,
	parameters: {
		a11y: {
			element: '[data-testid="close"]',
		},
		status: {
			type: 'ga',
		},
	},
};

export default meta;

type Story = StoryObj<typeof Close>;

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
