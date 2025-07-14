import type { Meta, StoryObj } from '@storybook/nextjs-vite';
import { LoadingIndicator } from './LoadingIndicator';

const meta: Meta<typeof LoadingIndicator> = {
  title: 'Components/LoadingIndicator',
  component: LoadingIndicator,
  tags: ['autodocs'],
  parameters: {
    docs: {
      description: {
        component: `⚠️ The \`LoadingIndicator\` uses absolute positioning, so it must be placed inside a container with \`relative\` positioning.

This story demonstrates proper usage with a \`div\` using Tailwind classes: \`relative mx-auto mt-24 w-52 h-12\`.`,
      },
    },
  },
};

export default meta;

type Story = StoryObj<typeof LoadingIndicator>;

export const Default: Story = {
  render: () => (
    <div className="relative mx-auto mt-24 pt-2 w-52 h-12 border border-dashed border-gray-300">
      <LoadingIndicator />
    </div>
  ),
};
