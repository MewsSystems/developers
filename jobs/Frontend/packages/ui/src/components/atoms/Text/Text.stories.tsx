/*
 |-----------------------------------------------------------------------------
 | src/components/atoms/Text/Text.stories.tsx
 | v1.0.0
 |-----------------------------------------------------------------------------
 */

import type { Meta, StoryObj } from '@storybook/react';

import Text from './Text';
import { props } from './mocks';

const meta: Meta<typeof Text> = {
	title: 'Components/Atoms/Text',
	component: Text,
	argTypes: {
		colour: {
			control: {
				type: 'select',
			},
		},
		leading: {
			control: {
				type: 'select',
			},
		},
		schema: {
			control: {
				type: 'select',
			},
		},
		transform: {
			control: {
				type: 'select',
			},
		},
	},
	args: props,
	parameters: {
		a11y: {
			element: '[data-testid="text"]',
		},
		status: {
			type: 'ga',
		},
	},
};

export default meta;

type Story = StoryObj<typeof Text>;

export const Default: Story = {};
