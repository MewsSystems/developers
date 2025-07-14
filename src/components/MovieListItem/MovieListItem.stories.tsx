import type { Meta, StoryObj } from '@storybook/nextjs-vite';
import { MovieListItem } from './MovieListItem';
import type { MovieSearchResult } from '@/types/api';

const demoMovie: MovieSearchResult = {
  id: 42,
  title: 'The Example Movie',
  original_title: 'The Example Movie',
  overview:
    'Nam quis lacus maximus, condimentum tellus vel, consequat ipsum. Quisque eu venenatis nibh. Suspendisse' +
    ' potenti. Duis consequat nibh sit amet enim molestie hendrerit. Cras posuere ante nec suscipit maximus. ' +
    'Fusce tincidunt arcu velit. Cras tempus consectetur eleifend. Etiam aliquam nulla ut gravida luctus. ' +
    'Quisque vitae molestie mi. Integer sit amet arcu convallis, venenatis est at, ultricies dui. In hac ' +
    'habitasse platea dictumst. In orci tellus, commodo in molestie vitae, suscipit sed mi. Mauris tempus ' +
    'a dolor cursus pretium. Pellentesque vestibulum faucibus arcu, id malesuada neque tempus auctor. Ut id ' +
    'facilisis velit. Nam lorem enim, rutrum et dolor id, viverra vulputate odio. Nunc nec volutpat justo, ' +
    'nec vehicula leo. Nulla finibus congue lacus, a ultricies turpis vestibulum eu. Phasellus bibendum ' +
    'urna in augue porta, sed ornare ante mattis. Sed diam mauris, viverra venenatis molestie nec, convallis ' +
    'sit amet purus. Morbi luctus egestas dui, vitae viverra ligula faucibus consequat. Nam vitae turpis ' +
    'tempor, faucibus justo ut, posuere lacus. Duis in augue eget velit molestie mollis nec sit amet lectus. ' +
    'Donec euismod imperdiet odio, sed porta lacus blandit id. Ut a nibh et tellus tempor consectetur at eget ' +
    'odio. Praesent lacinia tincidunt risus id porta. Vestibulum scelerisque sollicitudin ex, a mattis augue ' +
    'ornare a.',
  release_date: '2023-06-21',
  vote_average: 7.3,
  vote_count: 217,
  poster_url: {
    default: '/storybook-assets/default.webp',
    sm: '/storybook-assets/sm.webp',
    md: '/storybook-assets/md.webp',
  },
};

const demoMovieOriginalDiff: MovieSearchResult = {
  ...demoMovie,
  title: 'The Translated Title',
  original_title: 'Le Titre Original',
};

const demoMovieNoPoster: MovieSearchResult = {
  ...demoMovie,
  poster_url: {
    default: null,
    sm: null,
    md: null,
  },
};

const DemoContainer = ({ children }: { children: React.ReactNode }) => (
  <div className="w-full flex justify-center">
    <div className="flex w-full max-w-[768px] bg-whites content-stretch justify-center p-2 sm:p-6">
      {children}
    </div>
  </div>
);

const meta: Meta<typeof MovieListItem> = {
  title: 'Components/MovieListItem',
  component: MovieListItem,
  tags: ['autodocs'],
  parameters: {
    layout: 'fullscreen',
    docs: {
      description: {
        component: `
Renders a single movie in a search/list context, including:
- A poster image that responds to breakpoints (uses \`MoviePoster\`).
- Title (with clickable link).
- Original title (if different).
- Release date.
- Vote score.
- Truncated overview.
- Responsive design & accessibility best practices.

**Poster breakpoints supported:** \`default\`, \`sm\`, \`md\`

---

## Usage

\`\`\`tsx
<MovieListItem movie={movie} search="matrix" page={1} />
\`\`\`

---

### What It Looks Like

- Poster or fallback icon on the left.
- Details and actions on the right.
- Clicking the title navigates (with search and page params).
- Gracefully handles missing poster/title/overview/votes.
        `,
      },
    },
  },
  argTypes: {
    search: { control: 'text' },
    page: { control: 'number' },
  },
};
export default meta;

type Story = StoryObj<typeof MovieListItem>;

// Use the demo container in each story's render function
export const Default: Story = {
  args: {
    movie: demoMovie,
    search: 'matrix',
    page: 1,
  },
  render: (args) => (
    <DemoContainer>
      <MovieListItem {...args} />
    </DemoContainer>
  ),
  parameters: {
    docs: {
      description: {
        story: 'A typical movie result with all information filled in.',
      },
    },
  },
};

export const OriginalTitleDiffers: Story = {
  args: {
    movie: demoMovieOriginalDiff,
    search: 'paris',
    page: 2,
  },
  render: (args) => (
    <DemoContainer>
      <MovieListItem {...args} />
    </DemoContainer>
  ),
  parameters: {
    docs: {
      description: {
        story: 'Shows a different original title.',
      },
    },
  },
};

export const NoPoster: Story = {
  args: {
    movie: demoMovieNoPoster,
    search: 'space',
    page: 1,
  },
  render: (args) => (
    <DemoContainer>
      <MovieListItem {...args} />
    </DemoContainer>
  ),
  parameters: {
    docs: {
      description: {
        story: 'Shows fallback icon and still renders gracefully when there is no poster.',
      },
    },
  },
};

export const NoReleaseDate: Story = {
  args: {
    movie: { ...demoMovie, release_date: '' },
    search: 'empty',
    page: 3,
  },
  render: (args) => (
    <DemoContainer>
      <MovieListItem {...args} />
    </DemoContainer>
  ),
  parameters: {
    docs: {
      description: {
        story: 'No overview will hide the description section.',
      },
    },
  },
};

export const NoVotes: Story = {
  args: {
    movie: { ...demoMovie, vote_count: 0 },
    search: 'empty',
    page: 3,
  },
  render: (args) => (
    <DemoContainer>
      <MovieListItem {...args} />
    </DemoContainer>
  ),
  parameters: {
    docs: {
      description: {
        story: 'No overview will hide the description section.',
      },
    },
  },
};

export const NoOverview: Story = {
  args: {
    movie: { ...demoMovie, overview: '' },
    search: 'empty',
    page: 3,
  },
  render: (args) => (
    <DemoContainer>
      <MovieListItem {...args} />
    </DemoContainer>
  ),
  parameters: {
    docs: {
      description: {
        story: 'No overview will hide the description section.',
      },
    },
  },
};
