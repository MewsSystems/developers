import type { Meta, StoryObj } from '@storybook/nextjs-vite';
import { useState } from 'react';
import { Pagination } from './Pagination';
import type { PaginationProps } from './Pagination';

const meta: Meta<typeof Pagination> = {
  title: 'Components/Pagination',
  component: Pagination,
  tags: ['autodocs'],
  args: {
    totalPages: 10,
    currentPage: 1,
    search: 'storybook',
    readonly: false,
    disableKeyboardNav: false,
  },
  argTypes: {
    currentPage: { control: { type: 'number', min: 1 } },
    totalPages: { control: { type: 'number', min: 1 } },
    search: { control: 'text' },
    readonly: { control: 'boolean' },
    disableKeyboardNav: { control: 'boolean' },
    onPageChange: { action: 'page changed' },
  },
};
export default meta;

type Story = StoryObj<typeof Pagination>;

function ControlledPagination(args: PaginationProps) {
  const [page, setPage] = useState(args.currentPage);
  return <Pagination {...args} currentPage={page} onPageChange={setPage} />;
}

export const Default: Story = {
  args: {
    currentPage: 1,
    totalPages: 10,
    search: 'storybook',
  },
  render: (args) => <ControlledPagination {...args} />,
};

export const MiddlePage: Story = {
  args: {
    currentPage: 5,
    totalPages: 10,
    search: 'middle',
  },
  render: (args) => <ControlledPagination {...args} />,
};

export const LastPage: Story = {
  args: {
    currentPage: 10,
    totalPages: 10,
    search: 'last',
  },
  render: (args) => <ControlledPagination {...args} />,
};

export const FewPages: Story = {
  args: {
    currentPage: 1,
    totalPages: 3,
    search: 'few',
  },
  render: (args) => <ControlledPagination {...args} />,
};

export const DoubleDigitPageNumbers: Story = {
  args: {
    currentPage: 15,
    totalPages: 20,
    search: 'double-digits',
  },
  render: (args) => <ControlledPagination {...args} />,
};

export const TripleDigitPageNumbers: Story = {
  args: {
    currentPage: 150,
    totalPages: 200,
    search: 'triple-digits',
  },
  render: (args) => <ControlledPagination {...args} />,
};

export const Readonly: Story = {
  args: {
    currentPage: 3,
    totalPages: 10,
    search: 'readonly',
    readonly: true,
  },
  render: (args) => <Pagination {...args} onPageChange={() => {}} />,
  parameters: {
    docs: {
      description: {
        story: 'Allows the pagination component to display page state without interaction',
      },
    },
  },
};

export const DisabledKeyboardNav: Story = {
  args: {
    currentPage: 3,
    totalPages: 10,
    search: 'disabled-keyboard',
    disableKeyboardNav: true,
  },
  render: (args) => <ControlledPagination {...args} />,
  parameters: {
    docs: {
      description: {
        story:
          `Keyboard navigation is disabled. Pagination links cannot be tabbed to or focused using the keyboard. ` +
          'Usecase - We have a pagination component at the top of our search page and one at bottom, ' +
          `using this on the top pagination component improves the experience for keyboard navigation and screen readers`,
      },
    },
  },
};
