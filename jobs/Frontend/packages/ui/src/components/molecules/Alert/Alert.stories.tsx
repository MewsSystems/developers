/*
 |-----------------------------------------------------------------------------
 | src/components/molecules/Alert/Alert.stories.tsx
 | v1.0.0
 |-----------------------------------------------------------------------------
 */

import type { Meta, StoryObj } from '@storybook/react';

import Alert, { type IAlertProps } from './index';

const props: IAlertProps = {
	text: 'Lorem ipsum dolor sit amet, consectetur adipiscing elit',
	variant: undefined,
};

const meta: Meta<typeof Alert> = {
	title: 'Components/Molecules/Alert',
	component: Alert,
	argTypes: {
		variant: {
			control: {
				type: 'select',
			},
		},
	},
	args: props,
	parameters: {
		a11y: {
			element: '[data-testid="alert"]',
		},
		status: {
			type: 'ga',
		},
	},
};

export default meta;

type Story = StoryObj<typeof Alert>;

export const Error: Story = {
	args: {
		variant: 'error',
	},
};

export const Info: Story = {
	args: {
		variant: 'info',
	},
};

export const Success: Story = {
	args: {
		variant: 'success',
	},
};

export const Warning: Story = {
	args: {
		variant: 'warning',
	},
};
