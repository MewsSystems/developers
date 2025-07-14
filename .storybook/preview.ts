import type { Preview } from '@storybook/nextjs-vite';
import '../src/app/globals.css';
import { viewports } from './viewports';

const preview: Preview = {
  parameters: {
    controls: {
      matchers: {
        color: /(background|color)$/i,
        date: /Date$/i,
      },
    },

    a11y: {
      test: 'todo',
    },

    viewport: {
      options: viewports,
      defaultViewport: 'responsive',
    },
  },
};

export default preview;
