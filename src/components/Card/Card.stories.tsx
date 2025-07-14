import type { Meta, StoryObj } from '@storybook/nextjs-vite';
import { Card } from './Card';

const meta: Meta<typeof Card> = {
  title: 'Components/Card',
  component: Card,
  tags: ['autodocs'],
  parameters: {
    docs: {
      description: {
        component:
          'A flexible container with default styles to provide an outline. Accepts any valid `<div>` props and children.',
      },
    },
  },
  argTypes: {
    className: {
      description: 'Additional Tailwind CSS classes for the card.',
      control: { type: 'text' },
    },
    children: {
      description: 'Content to render inside the card.',
      control: false,
    },
  },
};
export default meta;

type Story = StoryObj<typeof Card>;

export const Default: Story = {
  args: {
    children: 'This is a Card.',
  },
};

export const WithCustomClass: Story = {
  args: {
    children: 'This card has a custom class.',
    className: 'text-red-600 border-3 p-10',
  },
};
