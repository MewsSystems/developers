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
The **AccessibleResultsSummary** component provides a concise, accessible summary of search results for assistive technology users. It is built to be used with the **ResultSummary** component.
It automatically updates to announce result changes via \`aria-live="polite"\`, and uses a short timer to clear and then reset the text content, ensuring that screen readers reliably announce the update—even when the summary string is similar or identical to the previous one.

**Tip:** The \`addSearchGuidance\` prop adds a visually hidden (\`sr-only\`) guidance message for users, for enhanced accessibility on focus or as-needed.
        `,
      },
    },
  },
  argTypes: {
    currentPage: {
      control: { type: 'number', min: 1 },
      description: 'The current page being viewed',
    },
    totalPages: {
      control: { type: 'number', min: 1 },
      description: 'The total number of result pages',
    },
    totalItems: { control: { type: 'number', min: 0 }, description: 'The total number of results' },
    pageSize: { control: { type: 'number', min: 1 }, description: 'Results shown per page' },
    addSearchGuidance: {
      control: 'boolean',
      description:
        'If true, appends a visually-hidden guidance message for assistive tech (screen readers) only.',
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
          'Shows the summary with the visually-hidden search guidance appended for accessibility. This is not visible, but is available to screen readers.',
      },
    },
  },
};
