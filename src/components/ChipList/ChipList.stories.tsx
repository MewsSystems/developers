import type { Meta, StoryObj } from '@storybook/nextjs-vite';
import { ChipList } from './ChipList';

const meta: Meta<typeof ChipList> = {
  title: 'Components/ChipList',
  component: ChipList,
  tags: ['autodocs'],
  parameters: {
    docs: {
      description: {
        component:
          'Displays a list of items styled as “chips”. Supports direction (row or column), background/text color, custom class names, and an optional title.',
      },
    },
  },
  argTypes: {
    title: { control: 'text', description: 'Optional section title.' },
    items: { control: { type: 'object' }, description: 'Array of items to display as chips.' },
    bgColor: { control: 'text', description: 'Tailwind background color class for chips.' },
    textColor: { control: 'text', description: 'Tailwind text color class for chips.' },
    direction: {
      control: { type: 'inline-radio', options: ['row', 'col'] },
      description: 'Display direction.',
    },
    className: { control: 'text', description: 'Custom class for the wrapper.' },
  },
};
export default meta;

type Story = StoryObj<typeof ChipList>;

export const Default: Story = {
  args: {
    title: 'Genres',
    items: ['Drama', 'Comedy', 'Adventure'],
  },
};

export const Vertical: Story = {
  args: {
    title: 'Categories',
    items: ['Alpha', 'Beta', 'Gamma'],
    direction: 'col',
  },
};

export const CustomColors: Story = {
  args: {
    title: 'Tags',
    items: ['Important', 'Notice'],
    bgColor: 'bg-green-100',
    textColor: 'text-green-700',
  },
};

export const NoTitle: Story = {
  args: {
    items: ['One', 'Two', 'Three'],
  },
};

export const Empty: Story = {
  args: {
    items: [],
    title: 'Nothing Here',
  },
  parameters: {
    docs: {
      description: {
        story: 'Returns nothing if `items` is empty.',
      },
    },
  },
};
