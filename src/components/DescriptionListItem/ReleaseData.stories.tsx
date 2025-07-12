import type { Meta, StoryObj } from '@storybook/nextjs-vite';
import { ReleaseDate } from './ReleaseDate';

const meta: Meta<typeof ReleaseDate> = {
  title: 'Components/DescriptionList/ReleaseDate',
  component: ReleaseDate,
  tags: ['autodocs'],
  parameters: {
    docs: {
      description: {
        component: `\`ReleaseDate\` is meant to be rendered inside a \`<dl>\` element, as it returns a \`<dt>\` and \`<dd>\` pair via the \`DescriptionListItem\` component.

It formats the given ISO date using \`<time>\`, and applies different styles when \`isSmall\` is set to true.`,
      },
    },
  },
};

export default meta;

type Story = StoryObj<typeof ReleaseDate>;

const Wrapper = ({ children }: { children: React.ReactNode }) => (
  <dl className="max-w-md p-4 mx-auto bg-white border border-gray-200 rounded-md space-y-2">
    {children}
  </dl>
);

export const WithValidDate: Story = {
  render: () => (
    <Wrapper>
      <ReleaseDate date="2024-12-25" />
    </Wrapper>
  ),
  parameters: {
    docs: {
      source: {
        code: `
<dl>
  <ReleaseDate date="2024-12-25" />
</dl>
        `.trim(),
      },
    },
  },
};

export const WithMissingDate: Story = {
  render: () => (
    <Wrapper>
      <ReleaseDate date="" />
    </Wrapper>
  ),
  parameters: {
    docs: {
      source: {
        code: `
<dl>
  <ReleaseDate date="" />
</dl>
        `.trim(),
      },
    },
  },
};

export const SmallVariant: Story = {
  render: () => (
    <Wrapper>
      <ReleaseDate date="2024-12-25" isSmall />
    </Wrapper>
  ),
  parameters: {
    docs: {
      source: {
        code: `
<dl>
  <ReleaseDate date="2024-12-25" isSmall />
</dl>
        `.trim(),
      },
    },
  },
};
