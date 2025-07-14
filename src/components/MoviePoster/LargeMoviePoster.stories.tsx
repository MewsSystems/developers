import type { Meta, StoryObj } from '@storybook/nextjs-vite';
import { LargeMoviePoster } from './LargeMoviePoster';

const posterAll = {
  default: '/storybook-assets/default-w185.webp',
  sm: '/storybook-assets/sm-w342.webp',
  lg: '/storybook-assets/lg-w500.webp',
};

const posterOnlyDefault = {
  default: '/storybook-assets/default-w185.webp',
  sm: null,
  lg: null,
};

const posterOnlySm = {
  default: null,
  sm: '/storybook-assets/sm-w342.webp',
  lg: null,
};

const posterOnlyLg = {
  default: null,
  sm: null,
  lg: '/storybook-assets/lg-w500.webp',
};

const posterNone = {
  default: null,
  sm: null,
  lg: null,
};

const meta: Meta<typeof LargeMoviePoster> = {
  title: 'Components/LargeMoviePoster',
  component: LargeMoviePoster,
  tags: ['autodocs'],
  parameters: {
    layout: 'centered',
    docs: {
      description: {
        component: `
Demonstrates per-breakpoint poster rendering and fallback icon logic for large movie posters.

| Breakpoint      | Image Present? | Shows \`<img>\` | Shows Icon | Fallback bg/center?      |
|-----------------|:--------------:|:--------------:|:----------:|:-------------------------|
| default (xs)    | default        | Yes            | No         | No (unless missing)      |
| sm (≥640px)     | sm             | Yes            | No         | No (unless missing)      |
| lg (≥1024px)    | lg             | Yes            | No         | No (unless missing)      |

**Usage Example:**

\`\`\`tsx
<LargeMoviePoster
  posterUrl={{
    default: '/storybook-assets/default-w185.webp',
    sm: '/storybook-assets/sm-w342.webp',
    lg: '/storybook-assets/lg-w500.webp',
  }}
  alt="Large Movie Poster"
/>
\`\`\`
        `,
      },
    },
  },
  argTypes: {
    posterUrl: { control: 'object' },
    alt: { control: 'text' },
  },
};
export default meta;

type Story = StoryObj<typeof LargeMoviePoster>;

const DemoContainer = ({ children }: { children: React.ReactNode }) => (
  <div className="p-6 flex flex-col items-center gap-8">{children}</div>
);

export const AllBreakpoints: Story = {
  render: () => (
    <DemoContainer>
      <h3>
        Try resizing the Storybook canvas to see the LargeMoviePoster respond at different
        breakpointDefinition!
      </h3>
      <LargeMoviePoster posterUrl={posterAll} alt="All breakpointDefinition image" />
    </DemoContainer>
  ),
  parameters: {
    docs: {
      description: {
        story: `
**Code Example:**

\`\`\`tsx
<LargeMoviePoster
  posterUrl={{
    default: '/storybook-assets/default-w185.webp',
    sm: '/storybook-assets/sm-w342.webp',
    lg: '/storybook-assets/lg-w500.webp',
  }}
  alt="All breakpointDefinition image"
/>
\`\`\`
        `,
      },
    },
  },
};

export const OnlyDefault: Story = {
  args: {
    posterUrl: posterOnlyDefault,
    alt: 'Only default image',
  },
  render: (args) => (
    <DemoContainer>
      <h3>
        Only <code>default</code> is present
      </h3>
      <LargeMoviePoster {...args} />
    </DemoContainer>
  ),
};

export const OnlySm: Story = {
  args: {
    posterUrl: posterOnlySm,
    alt: 'Only sm image',
  },
  render: (args) => (
    <DemoContainer>
      <h3>
        Only <code>sm</code> is present
      </h3>
      <LargeMoviePoster {...args} />
    </DemoContainer>
  ),
};

export const OnlyLg: Story = {
  args: {
    posterUrl: posterOnlyLg,
    alt: 'Only lg image',
  },
  render: (args) => (
    <DemoContainer>
      <h3>
        Only <code>lg</code> is present
      </h3>
      <LargeMoviePoster {...args} />
    </DemoContainer>
  ),
};

export const None: Story = {
  args: {
    posterUrl: posterNone,
    alt: 'No images',
  },
  render: (args) => (
    <DemoContainer>
      <h3>No images present, fallback icon shown at all breakpointDefinition</h3>
      <LargeMoviePoster {...args} />
    </DemoContainer>
  ),
};
