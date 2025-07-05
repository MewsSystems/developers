import type { Meta, StoryObj } from '@storybook/nextjs-vite';
import { MoviePoster } from './MoviePoster';

const posterAll = {
  default: '/storybook-assets/default.webp',
  sm: '/storybook-assets/sm.webp',
  md: '/storybook-assets/md.webp',
};

const posterOnlyDefault = {
  default: '/storybook-assets/default.webp',
  sm: null,
  md: null,
};

const posterOnlySm = {
  default: null,
  sm: '/storybook-assets/sm.webp',
  md: null,
};

const posterOnlyMd = {
  default: null,
  sm: null,
  md: '/storybook-assets/md.webp',
};

const posterNone = {
  default: null,
  sm: null,
  md: null,
};

const meta: Meta<typeof MoviePoster> = {
  title: 'Components/MoviePoster',
  component: MoviePoster,
  tags: ['autodocs'],
  parameters: {
    layout: 'centered',
    docs: {
      description: {
        component: `
Demonstrates per-breakpoint poster rendering and fallback icon logic.

**Summary Table:**

| Breakpoint      | Image Present? | Shows \`<img>\` | Shows Icon | Fallback bg/center?      |
|-----------------|:--------------:|:--------------:|:----------:|:-------------------------|
| default (xs)    | default        | Yes            | No         | No (unless missing)      |
| sm (≥640px)     | sm             | Yes            | No         | No (unless missing)      |
| md (≥768px)     | md             | Yes            | No         | No (unless missing)      |

**Usage:**

\`\`\`tsx
<MoviePoster
  posterUrl={{
    default: '/storybook-assets/default.webp',
    sm: '/storybook-assets/sm.webp',
    md: '/storybook-assets/md.webp',
  }}
  alt="My Movie Poster"
/>
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

type Story = StoryObj<typeof MoviePoster>;

const DemoContainer = ({ children }: { children: React.ReactNode }) => (
  <div className="p-6 flex flex-col items-center gap-8">{children}</div>
);

export const AllBreakpoints: Story = {
  render: () => (
    <DemoContainer>
      <h3>
        Try resizing the Storybook canvas to see the MoviePoster respond at different breakpoints!
      </h3>
      <MoviePoster posterUrl={posterAll} alt="All breakpoints image" />
    </DemoContainer>
  ),
  parameters: {
    docs: {
      description: {
        story: `
**Code Example:**

\`\`\`tsx
<MoviePoster
  posterUrl={{
    default: '/storybook-assets/default.webp',
    sm: '/storybook-assets/sm.webp',
    md: '/storybook-assets/md.webp',
  }}
  alt="All breakpoints image"
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
      <MoviePoster {...args} />
    </DemoContainer>
  ),
  parameters: {
    docs: {
      description: {
        story: `
**Code Example:**

\`\`\`tsx
<MoviePoster
  posterUrl={{ default: '/storybook-assets/default.webp', sm: null, md: null }}
  alt="Only default image"
/>
\`\`\`
        `,
      },
    },
  },
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
      <MoviePoster {...args} />
    </DemoContainer>
  ),
  parameters: {
    docs: {
      description: {
        story: `
**Code Example:**

\`\`\`tsx
<MoviePoster
  posterUrl={{ default: null, sm: '/storybook-assets/sm.webp', md: null }}
  alt="Only sm image"
/>
\`\`\`
        `,
      },
    },
  },
};

export const OnlyMd: Story = {
  args: {
    posterUrl: posterOnlyMd,
    alt: 'Only md image',
  },
  render: (args) => (
    <DemoContainer>
      <h3>
        Only <code>md</code> is present
      </h3>
      <MoviePoster {...args} />
    </DemoContainer>
  ),
  parameters: {
    docs: {
      description: {
        story: `
**Code Example:**

\`\`\`tsx
<MoviePoster
  posterUrl={{ default: null, sm: null, md: '/storybook-assets/md.webp' }}
  alt="Only md image"
/>
\`\`\`
        `,
      },
    },
  },
};

export const None: Story = {
  args: {
    posterUrl: posterNone,
    alt: 'No images',
  },
  render: (args) => (
    <DemoContainer>
      <h3>No images present, fallback icon shown at all breakpoints</h3>
      <MoviePoster {...args} />
    </DemoContainer>
  ),
  parameters: {
    docs: {
      description: {
        story: `
**Code Example:**

\`\`\`tsx
<MoviePoster
  posterUrl={{ default: null, sm: null, md: null }}
  alt="No images"
/>
\`\`\`
        `,
      },
    },
  },
};
