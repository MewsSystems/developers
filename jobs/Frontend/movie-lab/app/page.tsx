import { getMovieList } from "@/src/backend/getMovies";
import { MovieType } from "@/src/types/movie";
import MovieLab from "@/src/components/MovieLab";

async function getData({
                         page,
                         search,
                       }: {
  page: number;
  search: string;
}): Promise<{ data: MovieType[], totalPages: number }> {
  try {
    if (page <= 0) {
      throw new Error('Invalid page number');
    }

    const sanitizedSearch = search.replace(/[&<>"']/g, '');
    let allData: MovieType[] = [];
    let totalPages = 0;

    // Fetch data for all pages up to the current page
    for (let currentPage = 1; currentPage <= page; currentPage++) {
      const res = await getMovieList({ page: currentPage, search: sanitizedSearch });
      totalPages = res.total_pages;
      allData = [...allData, ...res.results];
    }

    return { data: allData, totalPages: totalPages };
  } catch (error: any) {
    console.error('Error fetching data:', error.message);
    throw error;
  }
}

export default async function Home({
                                     searchParams,
                                   }: {
  searchParams: { [key: string]: string | undefined };
}) {
  const page = parseInt(searchParams["page"] ?? "1", 10);
  const search = searchParams["q"] ?? "";

  const response = await getData({
    page,
    search,
  });

  return (
    <MovieLab data={response.data} totalPages={response.totalPages} />
  );
}
