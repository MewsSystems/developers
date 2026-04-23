import { MovieType } from "@/src/types/movie";

const apiKey = '03b8572954325680265531140190fd2a';

export async function getMovieList({ page, search }: {
 page: number;
  search: string;
}): Promise<{ total_pages: number; results: MovieType[] }> {
  let url = search
    ? `https://api.themoviedb.org/3/search/movie?api_key=${apiKey}&query=${search}`
    : `https://api.themoviedb.org/3/movie/popular?api_key=${apiKey}`;

  if (page !== undefined) {
    url += `&page=${page}`;
  }

  const res = await fetch(url).then(response => response.json());

  return res;
}
