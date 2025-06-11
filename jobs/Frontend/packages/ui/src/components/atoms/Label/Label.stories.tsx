/*
 |-----------------------------------------------------------------------------
 | src/components/atoms/Label/Label.stories.tsx
 | v1.0.0
 |-----------------------------------------------------------------------------
 */

import type { Meta, StoryObj } from '@storybook/react';

import Label from './Label';
import { props } from './mocks';

const meta: Meta<typeof Label> = {
	title: 'Components/Atoms/Label',
	component: Label,
	args: props,
	parameters: {
		a11y: {
			element: '[data-testid="label"]',
		},
		status: {
			type: 'ga',
		},
	},
};

export default meta;

type Story = StoryObj<typeof Label>;

export const Default: Story = {};

export const Disabled: Story = {
	args: {
		isDisabled: true,
	},
	name: 'Is disabled',
};

export const Hidden: Story = {
	args: {
		isHidden: true,
	},
	name: 'Is hidden',
};
