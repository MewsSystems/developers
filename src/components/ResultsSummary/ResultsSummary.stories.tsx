import type { Meta, StoryObj } from '@storybook/nextjs-vite';
import { ResultsSummary } from './ResultsSummary';
import { AccessibleResultsSummary } from './AccessibleResultsSummary';

const meta: Meta<typeof ResultsSummary> = {
  title: 'Components/ResultsSummary',
  component: ResultsSummary,
  tags: ['autodocs'],
  parameters: {
    docs: {
      description: {
        component: `
**ResultsSummary** is a container component designed to wrap search result summaries, such as \`AccessibleResultsSummary\`.  
When the container receives focus, it sets \`addSearchGuidance\` on any child \`AccessibleResultsSummary\` to enhance screen reader guidance.  
You can use this container to coordinate accessible updates in paginated or filterable search UIs.
        `,
      },
    },
  },
  argTypes: {
    children: { control: false },
  },
};

export default meta;

type Story = StoryObj<typeof ResultsSummary>;

// Helper: Demo focus state for docs
const DemoInstructions = () => (
  <div className="mb-2 text-sm text-stone-600">
    <span className="font-semibold">Demo:</span>
    Click on the results summary box below to trigger the <code>addSearchGuidance</code> message for
    screen readers (it will be rendered in a visually-hidden span).
  </div>
);

export const WithAccessibleResultsSummary: Story = {
  render: (args) => (
    <>
      <DemoInstructions />
      <ResultsSummary {...args}>
        <AccessibleResultsSummary currentPage={2} totalPages={4} totalItems={80} pageSize={20} />
      </ResultsSummary>
    </>
  ),
  parameters: {
    docs: {
      description: {
        story:
          'Shows a results summary with accessibility guidance (for screen readers) enabled when the container is focused.',
      },
    },
  },
};

export const WithCustomChild: Story = {
  render: (args) => (
    <ResultsSummary {...args}>
      <div style={{ padding: '1rem' }}>You can use any child component here.</div>
    </ResultsSummary>
  ),
  parameters: {
    docs: {
      description: {
        story: 'ResultsSummary can wrap any content, not just AccessibleResultsSummary.',
      },
    },
  },
};
