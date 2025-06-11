/*
 |-----------------------------------------------------------------------------
 | src/components/atoms/Loader/Loader.stories.tsx
 | v1.0.0
 |-----------------------------------------------------------------------------
 */

import type { Meta, StoryObj } from '@storybook/react';

import Loader from './Loader';

const meta: Meta<typeof Loader> = {
	title: 'Components/Atoms/Loader',
	component: Loader,
	parameters: {
		a11y: {
			element: '[data-testid="loader"]',
		},
		status: {
			type: 'ga',
		},
	},
};

export default meta;

type Story = StoryObj<typeof Loader>;

export const Default: Story = {};
