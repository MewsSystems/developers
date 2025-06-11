/*
 |-----------------------------------------------------------------------------
 | src/components/atoms/Error/Error.stories.tsx
 | v1.0.0
 |-----------------------------------------------------------------------------
 */

import type { Meta, StoryObj } from '@storybook/react';

import Error from './Error';
import { props } from './mocks';

const meta: Meta<typeof Error> = {
	title: 'Components/Atoms/Error',
	component: Error,
	args: props,
	parameters: {
		a11y: {
			element: '[data-testid="error"]',
		},
		status: {
			type: 'ga',
		},
	},
};

export default meta;

type Story = StoryObj<typeof Error>;

export const Default: Story = {};
