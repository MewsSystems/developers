const path = require("path");
const withPlugins = require("next-compose-plugins");
const withPWAInit = require("next-pwa");
const withNextIntl = require("next-intl/plugin")();
const { withSentryConfig } = require("@sentry/nextjs");

const isDevelopment = process.env.NODE_ENV === "development";

const withPWA = withPWAInit({
  disable: isDevelopment,
  dest: "public",
  exclude: [
    ({ asset }) => {
      if (
        asset.name.startsWith("server/") ||
        asset.name.match(
          /^((app-|^)build-manifest\.json|react-loadable-manifest\.json)$/
        ) ||
        (isDevelopment && !asset.name.startsWith("static/runtime/"))
      ) {
        return true;
      }
      return false;
    },
  ],
});

/** @type {import('next').NextConfig} */
const nextConfig = {
  sentry: {
    hideSourceMaps: true,
    widenClientFileUpload: true,
  },
  compiler: {
    styledComponents: true,
  },
  webpack(config) {
    if (!isDevelopment) {
      const registerJs = path.join(
        path.dirname(require.resolve("next-pwa")),
        "register.js"
      );
      const entry = config.entry;
      config.entry = () =>
        entry().then((entries) => {
          if (
            entries["main-app"] &&
            !entries["main-app"].includes(registerJs)
          ) {
            if (Array.isArray(entries["main-app"])) {
              entries["main-app"].unshift(registerJs);
            } else if (typeof entries["main-app"] === "string") {
              entries["main-app"] = [registerJs, entries["main-app"]];
            }
          }
          return entries;
        });
    }
    return config;
  },
};

const sentryWebpackPluginOptions = {
  org: process.env.SENTRY_ORG,
  project: process.env.SENTRY_WEB_PROJECT,
  authToken: process.env.SENTRY_AUTH_TOKEN,
  silent: true,
};
const withSentry = (config) =>
  withSentryConfig(config, sentryWebpackPluginOptions);

const plugins = [withPWA, withNextIntl, withSentry];
module.exports = withPlugins(plugins, nextConfig);
