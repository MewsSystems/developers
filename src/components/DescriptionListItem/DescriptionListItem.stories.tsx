import type { Meta, StoryObj } from '@storybook/nextjs-vite';
import { DescriptionListItem } from './DescriptionListItem';

const meta: Meta<typeof DescriptionListItem> = {
  title: 'Components/DescriptionList/DescriptionListItem',
  component: DescriptionListItem,
  tags: ['autodocs'],
  parameters: {
    docs: {
      description: {
        component: `\`DescriptionListItem\` is meant to be used inside a \`<dl>\` element. It renders a \`<dt>\` for the term and a \`<dd>\` for the detail.

⚠️ Always wrap this component inside a \`<dl>\` — this story shows it using a wrapper for layout, but the code examples are shown explicitly with \`<dl>\`.`,
      },
    },
  },
};

export default meta;

type Story = StoryObj<typeof DescriptionListItem>;

const Wrapper = ({ children }: { children: React.ReactNode }) => (
  <dl className="max-w-md p-4 mx-auto bg-white border border-gray-200 rounded-md space-y-2">
    {children}
  </dl>
);

export const Basic: Story = {
  render: () => (
    <Wrapper>
      <DescriptionListItem term="Director" detail="Christopher Nolan" />
    </Wrapper>
  ),
  parameters: {
    docs: {
      source: {
        code: `
<dl>
  <DescriptionListItem term="Director" detail="Christopher Nolan" />
</dl>
        `.trim(),
      },
    },
  },
};

export const BasicInline: Story = {
  render: () => (
    <Wrapper>
      <DescriptionListItem term="Director: " detail="Christopher Nolan" detailClassName="inline" />
    </Wrapper>
  ),
  parameters: {
    docs: {
      source: {
        code: `
<dl>
  <DescriptionListItem term="Director" detail="Christopher Nolan" detailClassName="inline" />
</dl>
        `.trim(),
      },
    },
  },
};

export const WithCustomClasses: Story = {
  render: () => (
    <Wrapper>
      <DescriptionListItem
        term="Rating"
        detail="PG-13"
        termClassName="text-red-600"
        detailClassName="text-gray-500"
      />
    </Wrapper>
  ),
  parameters: {
    docs: {
      source: {
        code: `
<dl>
  <DescriptionListItem
    term="Rating"
    detail="PG-13"
    termClassName="text-red-600"
    detailClassName="text-gray-500"
  />
</dl>
        `.trim(),
      },
    },
  },
};

export const WithReactNodeDetail: Story = {
  render: () => (
    <Wrapper>
      <DescriptionListItem
        term="More Info"
        detail={
          <a href="/movie" className="text-blue-600 underline">
            View
          </a>
        }
      />
    </Wrapper>
  ),
  parameters: {
    docs: {
      source: {
        code: `
<dl>
  <DescriptionListItem
    term="More Info"
    detail={
      <a href="/movie" className="text-blue-600 underline">
        View
      </a>
    }
  />
</dl>
        `.trim(),
      },
    },
  },
};
