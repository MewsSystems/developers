import type { Meta, StoryObj } from '@storybook/nextjs-vite';
import { ResponsiveImage } from './ResponsiveImage';
import type { BreakpointDefinition } from './ResponsiveImage';

const demoBreakpoints: BreakpointDefinition = {
  default: {
    src: '/storybook-assets/default-w185.webp',
    containerSize: 'w-[185px] h-[278px]',
  },
  sm: {
    src: '/storybook-assets/sm-w342.webp',
    containerSize: 'sm:w-[342px] sm:h-[513px]',
  },
  lg: {
    src: '/storybook-assets/lg-w500.webp',
    containerSize: 'lg:w-[500px] lg:h-[750px]',
  },
};

const demoDefault: BreakpointDefinition = {
  default: {
    src: '/storybook-assets/default-w185.webp',
    containerSize: 'w-[185px] h-[278px]',
  },
};

const demoNone: BreakpointDefinition = {
  default: {
    src: null,
    containerSize: 'w-[185px] h-[278px]',
  },
  sm: {
    src: null,
    containerSize: 'sm:w-[342px] sm:h-[513px]',
  },
  lg: {
    src: null,
    containerSize: 'lg:w-[500px] lg:h-[750px]',
  },
};

const meta: Meta<typeof ResponsiveImage> = {
  title: 'Components/ResponsiveImage',
  component: ResponsiveImage,
  tags: ['autodocs'],
  parameters: {
    layout: 'centered',
    docs: {
      description: {
        component: `
**ResponsiveImage**

A generic, highly-reusable component for rendering responsive images with per-breakpoint source support, fallback icon, and Tailwind container sizing.

> 💡 **Width and height props** should always match the \`w\` and \`h\` set on the \`default\` entry in \`containerSize\`.  
> E.g. \`w-[185px] h-[278px]\` means you should use \`width={185}\` and \`height={278}\`.

### Usage:

\`\`\`tsx
import { ResponsiveImage } from './ResponsiveImage';

<ResponsiveImage
  breakpointDefinition={{
    default: { src: '/img-default.webp', containerSize: 'w-[185px] h-[278px]' },
    sm: { src: '/img-sm.webp', containerSize: 'sm:w-[342px] sm:h-[513px]' },
    lg: { src: '/img-lg.webp', containerSize: 'lg:w-[500px] lg:h-[750px]' },
  }}
  alt="Some image"
  width={185}
  height={278}
/>
\`\`\`
        `,
      },
    },
  },
};
export default meta;

type Story = StoryObj<typeof ResponsiveImage>;

const DemoContainer = ({ children }: { children: React.ReactNode }) => (
  <div className="p-6 flex flex-col items-center gap-8 bg-white">{children}</div>
);

export const AllBreakpoints: Story = {
  args: {
    breakpointDefinition: demoBreakpoints,
    alt: 'All breakpoints image',
    width: 185,
    height: 278,
  },
  render: (args) => (
    <DemoContainer>
      <h3>All breakpoints defined (default, sm, lg). Resize the preview!</h3>
      <ResponsiveImage {...args} />
    </DemoContainer>
  ),
  parameters: {
    docs: {
      description: {
        story: `
Shows the image at all breakpoints.

**Supported breakpoints:**  
- **default (xs)**: \`/img-default.webp\`  
- **sm (≥640px)**: \`/img-sm.webp\`  
- **md (≥768px)**: \`/img-md.webp\`  
- **lg (≥1024px)**: \`/img-lg.webp\`  
- **xl (≥1280px)**: \`/img-xl.webp\`  
- **2xl (≥1536px)**: \`/img-2xl.webp\`  

*(In this story, only default, sm, and lg are provided as examples.)*

Try resizing the Storybook canvas to see the responsive image change!
        `,
      },
    },
  },
};

export const OnlyDefault: Story = {
  args: {
    breakpointDefinition: demoDefault,
    alt: 'Only default image',
    width: 185,
    height: 278,
  },
  render: (args) => (
    <DemoContainer>
      <h3>
        Only <code>default</code> image is provided
      </h3>
      <ResponsiveImage {...args} />
    </DemoContainer>
  ),
};

export const None: Story = {
  args: {
    breakpointDefinition: demoNone,
    alt: 'No images',
    width: 185,
    height: 278,
  },
  render: (args) => (
    <DemoContainer>
      <h3>No images present – fallback icon shown at all breakpoints</h3>
      <ResponsiveImage {...args} />
    </DemoContainer>
  ),
};
