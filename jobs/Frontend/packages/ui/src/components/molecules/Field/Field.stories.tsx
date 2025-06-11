/*
 |-----------------------------------------------------------------------------
 | src/components/atoms/Field/Field.stories.tsx
 | v1.0.0
 |-----------------------------------------------------------------------------
 */

import type { Meta, StoryObj } from '@storybook/react';
import Input from '@ui/components/atoms/Input';

import Field from './Field';
import { props } from './mocks';

const meta: Meta<typeof Field> = {
	title: 'Components/Molecules/Field',
	component: Field,
	args: props,
	parameters: {
		a11y: {
			element: '[data-testid="field"]',
		},
		status: {
			type: 'ga',
		},
	},
	render: (args) => (
		<Field {...args}>
			<Input id={args.id} isInvalid={!!args.error} name="Field" />
		</Field>
	),
};

export default meta;

type Story = StoryObj<typeof Field>;

export const Default: Story = {};

export const Error: Story = {
	args: {
		error: 'Lorem ipsum dolor sit amet, consectetur adipiscing elit',
	},
	name: 'With error',
};

export const Hint: Story = {
	args: {
		hint: 'Lorem ipsum dolor sit amet, consectetur adipiscing elit',
	},
	name: 'With hint',
};

export const HiddenLabel: Story = {
	args: {
		hasHiddenLabel: true,
	},
	name: 'Without label',
};
