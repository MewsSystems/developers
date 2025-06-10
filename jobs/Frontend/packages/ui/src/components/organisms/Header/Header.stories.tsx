/*
 |-----------------------------------------------------------------------------
 | src/components/organisms/Header/Header.stories.tsx
 | v1.0.0
 |-----------------------------------------------------------------------------
 */

import type { Meta, StoryObj } from '@storybook/react';

import Header from './Header';
import { props } from './mocks';

const meta: Meta<typeof Header> = {
	title: 'Components/Organisms/Header',
	component: Header,
	args: props,
	parameters: {
		a11y: {
			element: '[data-testid="header"]',
		},
		status: {
			type: 'ga',
		},
	},
};

export default meta;

type Story = StoryObj<typeof Header>;

export const Default: Story = {};
