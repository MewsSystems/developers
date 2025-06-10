/*
 |-----------------------------------------------------------------------------
 | src/components/atoms/Hint/Hint.stories.tsx
 | v1.0.0
 |-----------------------------------------------------------------------------
 */

import type { Meta, StoryObj } from '@storybook/react';

import Hint from './Hint';
import { props } from './mocks';

const meta: Meta<typeof Hint> = {
	title: 'Components/Atoms/Hint',
	component: Hint,
	args: props,
	parameters: {
		a11y: {
			element: '[data-testid="hint"]',
		},
		status: {
			type: 'ga',
		},
	},
};

export default meta;

type Story = StoryObj<typeof Hint>;

export const Default: Story = {};
