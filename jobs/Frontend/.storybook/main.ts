import type { StorybookConfig } from "@storybook/react-vite";

const config: StorybookConfig = {
  stories: ["../src/**/*.mdx", "../src/**/*.stories.@(js|jsx|mjs|ts|tsx)"],
  addons: [
    "@chromatic-com/storybook",
    "@storybook/addon-docs",
    "@storybook/addon-a11y",
  ],
  framework: {
    name: "@storybook/react-vite",
    options: {},
  },
  staticDirs: ["../public", "../src/assets"],
  // core: {
  //   builder: {
  //     name: "@storybook/builder-vite",
  //     options: {
  //       viteConfigPath: "./vite.base.config.js",
  //     },
  //   },
  // },
};
export default config;
