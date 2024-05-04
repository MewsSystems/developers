/** @type {import('next').NextConfig} */
const nextConfig = {
  compiler: {
    styledComponents: true,
  },
  images: {
    loader: "custom",
    loaderFile: "loader.ts",
    remotePatterns: [
      {
        protocol: "https",
        hostname: "image.tmdb.org",
        pathname: "/t/p/**",
      },
    ],
    imageSizes: [342, 780], // PosterSizes from the MovieDb
  },
};

export default nextConfig;
