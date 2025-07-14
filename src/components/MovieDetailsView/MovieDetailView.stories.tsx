import type { Meta, StoryObj } from '@storybook/nextjs-vite';
import { MovieDetailsView } from './MovieDetailsView';
import type { MovieDetailResponse } from '@/types/api';
import type { ReactNode } from 'react';

const DemoContainer = ({ children }: { children: ReactNode }) => (
  <div className="w-full flex justify-center">
    <div className="flex w-full max-w-[1024px] bg-whites content-stretch justify-center p-2 sm:p-6">
      {children}
    </div>
  </div>
);

const overview =
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
  'ornare a.';

const exampleMovie: MovieDetailResponse = {
  id: 123,
  title: 'Test Movie',
  original_title: 'Original Test Movie',
  tagline: 'This is a test tagline.',
  release_date: '2021-08-12',
  vote_average: 8.3,
  vote_count: 1234,
  status: 'Released',
  runtime: 142,
  spoken_languages: [{ english_name: 'English', iso_639_1: 'en', name: 'English' }],
  origin_country: ['US'],
  overview,
  genres: [
    { id: 1, name: 'Drama' },
    { id: 2, name: 'Adventure' },
  ],
  production_companies: [
    { id: 10, name: 'OpenAI Productions', origin_country: 'US', logo_path: null },
    { id: 11, name: 'AI Films', origin_country: 'GB', logo_path: null },
  ],
  production_countries: [
    { iso_3166_1: 'US', name: 'United States of America' },
    { iso_3166_1: 'GB', name: 'United Kingdom' },
  ],
  poster_url: {
    default: '/storybook-assets/default-w185.webp',
    sm: '/storybook-assets/sm-w342.webp',
    lg: '/storybook-assets/lg-w500.webp',
  },
};

const movieNoOriginalTitleOrTagline: MovieDetailResponse = {
  ...exampleMovie,
  original_title: 'Test Movie',
  tagline: '',
};

const movieMissingOptional: MovieDetailResponse = {
  ...exampleMovie,
  original_title: 'Test Movie',
  tagline: '',
  genres: [],
  production_companies: [],
  production_countries: [],
  spoken_languages: [],
  poster_url: {
    default: '',
    sm: '',
    lg: '',
  },
  overview: '',
};

const meta: Meta<typeof MovieDetailsView> = {
  title: 'Components/MovieDetailsView',
  component: MovieDetailsView,
  tags: ['autodocs'],
  parameters: {
    layout: 'fullscreen',
    docs: {
      description: {
        component:
          'A detailed view for displaying information about a specific movie, including genres, production, languages, and a large poster.',
      },
    },
  },
  decorators: [
    (Story) => (
      <DemoContainer>
        <Story />
      </DemoContainer>
    ),
  ],
};
export default meta;

type Story = StoryObj<typeof MovieDetailsView>;

export const Default: Story = {
  args: {
    movie: exampleMovie,
  },
};

export const NoOriginalTitleOrTagline: Story = {
  args: {
    movie: movieNoOriginalTitleOrTagline,
  },
};

export const MissingOptionalData: Story = {
  args: {
    movie: movieMissingOptional,
  },
};
