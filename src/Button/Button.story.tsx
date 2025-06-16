import type { Meta, StoryObj } from '@storybook/react';
import { Button } from './Button';

const meta: Meta<typeof Button> = {
    title: 'Button',
    component: Button,
    tags: ['autodocs'],
    parameters: {
        docs: {
            description: {
            component: 'Buttons are interactive elements that trigger actions.',
            },
        },
    },
};

export default meta;

type Story = StoryObj<typeof Button>;

export const Default: Story = {
  args: {
    label: 'Show more',
    handleLoadMore: () => console.log('Button clicked!')
  },
};