import type { Meta, StoryObj } from '@storybook/nextjs-vite';
import { MovieListSkeleton } from './MovieListSkeleton';

const meta: Meta<typeof MovieListSkeleton> = {
  title: 'Components/MovieListItem/MovieListSkeleton',
  component: MovieListSkeleton,
  tags: ['autodocs'],
  parameters: {
    docs: {
      description: {
        component:
          'Skeleton loader for a movie list. Renders a configurable number of skeleton items for loading states.',
      },
    },
  },
  argTypes: {
    itemNumber: {
      control: { type: 'number', min: 0, max: 20 },
      description: 'Number of skeleton items to render',
      defaultValue: 5,
    },
  },
};
export default meta;

type Story = StoryObj<typeof MovieListSkeleton>;

export const Default: Story = {
  args: {
    itemNumber: 1,
  },
  render: (args) => (
    <div className="flex flex-col gap-2">
      <MovieListSkeleton {...args} />
    </div>
  ),
};
