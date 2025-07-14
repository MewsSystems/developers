import type { Meta, StoryObj } from '@storybook/nextjs-vite';
import { ErrorMessage } from './ErrorMessage';

const meta: Meta<typeof ErrorMessage> = {
  title: 'Components/ErrorMessage',
  component: ErrorMessage,
  tags: ['autodocs'],
  args: {
    message: 'Something went wrong.',
  },
};

export default meta;

type Story = StoryObj<typeof ErrorMessage>;

export const Default: Story = {};
