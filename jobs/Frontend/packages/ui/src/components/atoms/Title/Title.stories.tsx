/*
 |-----------------------------------------------------------------------------
 | src/components/atoms/Title/Title.stories.tsx
 | v1.0.0
 |-----------------------------------------------------------------------------
 */

import type { Meta, StoryObj } from '@storybook/react';

import Title from './Title';
import { props } from './mocks';

const meta: Meta<typeof Title> = {
	title: 'Components/Atoms/Title',
	component: Title,
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
		level: {
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

type Story = StoryObj<typeof Title>;

export const Default: Story = {};
