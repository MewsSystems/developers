import { useState } from "react";
import Constants from "../../config/constants";
import { useLoaderData } from "react-router";

export default function MovieDetail() {
  //   const [isFetching, setIsFetching] = useState(true);
  const movie = useLoaderData();
  const imagePath = Constants.IMAGE_URL + "/" + movie.backdrop_path;
  return (
    <div className="flex flex-col items-center">
      <img
        className="h-auto max-w-full"
        src={imagePath}
        alt="image description"
      />

      <h1 className="mb-4 text-4xl font-extrabold leading-none tracking-tight text-gray-900 md:text-5xl lg:text-6xl dark:text-white">
        {movie.title}
      </h1>
      <p className="mb-6 text-lg font-normal text-gray-500 lg:text-xl sm:px-16 xl:px-48 dark:text-gray-400">
        {movie.overview}
      </p>
    </div>
  );
}
