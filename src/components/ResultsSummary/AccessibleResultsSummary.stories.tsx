import type { Meta, StoryObj } from '@storybook/nextjs-vite';
import { AccessibleResultsSummary } from './AccessibleResultsSummary';

const meta: Meta<typeof AccessibleResultsSummary> = {
  title: 'Components/ResultsSummary/AccessibleResultsSummary',
  component: AccessibleResultsSummary,
  tags: ['autodocs'],
  parameters: {
    docs: {
      description: {
        component: `
The **AccessibleResultsSummary** component provides a concise, accessible summary of search results for assistive technology users. It should be used alongside the **ResultsSummary** container.

Key features:
- Uses \`aria-live="polite"\` and \`aria-atomic="true"\` for reliable screen reader announcements.
- Separates **visible text** from **screen reader text**, ensuring accessibility and UX.
- Uses a delayed update pattern to ensure changes are picked up by assistive tech.
- \`addSearchGuidance\` adds extra instruction text for screen reader users.
- \`isHidden\` can be used to visually hide the component (keeping it accessible).

_Note: It is intended to be rendered persistently (not conditionally), and updated via props._
        `,
      },
    },
  },
  argTypes: {
    currentPage: {
      control: { type: 'number', min: 1 },
      description: 'The current results page',
    },
    totalPages: {
      control: { type: 'number', min: 1 },
      description: 'Total number of available pages',
    },
    totalItems: {
      control: { type: 'number', min: 0 },
      description: 'Total number of search results',
    },
    pageSize: {
      control: { type: 'number', min: 1 },
      description: 'Number of results per page',
    },
    addSearchGuidance: {
      control: 'boolean',
      description:
        'Adds an extra visually-hidden message for screen reader users about starting a new search.',
    },
    isHidden: {
      control: 'boolean',
      description: 'If true, hides the text.',
    },
  },
};

export default meta;

type Story = StoryObj<typeof AccessibleResultsSummary>;

export const Default: Story = {
  args: {
    currentPage: 1,
    totalPages: 3,
    totalItems: 60,
    pageSize: 20,
  },
};

export const MiddlePage: Story = {
  args: {
    currentPage: 2,
    totalPages: 3,
    totalItems: 60,
    pageSize: 20,
  },
};

export const LastPage: Story = {
  args: {
    currentPage: 3,
    totalPages: 3,
    totalItems: 60,
    pageSize: 20,
  },
};

export const NoResults: Story = {
  args: {
    currentPage: 1,
    totalPages: 1,
    totalItems: 0,
    pageSize: 20,
  },
};

export const WithSearchGuidance: Story = {
  args: {
    currentPage: 1,
    totalPages: 1,
    totalItems: 1,
    pageSize: 20,
    addSearchGuidance: true,
  },
  parameters: {
    docs: {
      description: {
        story:
          'Adds a screen-reader-only message that provides guidance on how to begin a new search. This message is not visible to sighted users.',
      },
    },
  },
};

export const HiddenFromSightedUsers: Story = {
  args: {
    currentPage: 2,
    totalPages: 5,
    totalItems: 100,
    pageSize: 20,
    isHidden: true,
    addSearchGuidance: true,
  },
  parameters: {
    docs: {
      description: {
        story: 'This version hides the summary',
      },
    },
  },
};
