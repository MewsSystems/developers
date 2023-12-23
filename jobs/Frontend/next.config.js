/** @type {import('next').NextConfig} */
const nextConfig = {
  reactStrictMode: true,
  images: {
    remotePatterns: [
      {
        protocol: 'https',
        hostname: 'image.tmdb.org',
      },
    ],
  },
  compiler: {
    styledComponents: {
      displayName: true,
    },
  },
}

module.exports = nextConfig
