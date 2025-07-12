import type { Meta, StoryObj } from '@storybook/nextjs-vite';
import { Score } from './Score';

const meta: Meta<typeof Score> = {
  title: 'Components/DescriptionList/Score',
  component: Score,
  tags: ['autodocs'],
  parameters: {
    docs: {
      description: {
        component: `\`Score\` is a presentational component designed to be rendered inside a \`<dl>\`. It uses \`<dt>\` and \`<dd>\` under the hood via \`DescriptionListItem\`.

It displays a formatted score and vote count, and supports a compact layout via the \`isSmall\` prop.`,
      },
    },
  },
};

export default meta;

type Story = StoryObj<typeof Score>;

const Wrapper = ({ children }: { children: React.ReactNode }) => (
  <dl className="max-w-md p-4 mx-auto bg-white border border-gray-200 rounded-md space-y-2">
    {children}
  </dl>
);

export const Default: Story = {
  render: () => (
    <Wrapper>
      <Score score={8.7} count={1342} />
    </Wrapper>
  ),
  parameters: {
    docs: {
      source: {
        code: `
<dl>
  <Score score={8.7} count={1342} />
</dl>
        `.trim(),
      },
    },
  },
};

export const SmallVariant: Story = {
  render: () => (
    <Wrapper>
      <Score score={7.3} count={378} isSmall />
    </Wrapper>
  ),
  parameters: {
    docs: {
      source: {
        code: `
<dl>
  <Score score={7.3} count={378} isSmall />
</dl>
        `.trim(),
      },
    },
  },
};
