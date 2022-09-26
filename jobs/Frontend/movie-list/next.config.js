/** @type {import('next').NextConfig} */
const nextConfig = {
  reactStrictMode: true,
  swcMinify: true,
  compiler: {
    styledComponents: true
  },
  images: {
    domains:["image.tmdb.org"]
  },
  redirects: async () => {
    return [
      {
        source: "/",
        destination: "/movies",
        permanent: true
      }
    ]
  }
}

module.exports = nextConfig
