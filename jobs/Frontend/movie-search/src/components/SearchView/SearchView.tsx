import Search from "./Search";
import MovieCard from "./MovieCard";
import { QueryClientProvider } from "@tanstack/react-query";
import { queryClient } from "@/hooks/queryClient";
import MoviesDetailsModal from "../MoviesDetailsView/MoviesDetailsModal";
import { useAppSelector } from "@/hooks/store";
export default function SearchView() {
  const openModal = useAppSelector((state) => state.modalState.isOpen);
  return (
    <>
      <QueryClientProvider client={queryClient}>
        <div className="flex flex-col w-full min-h-screen p-4">
          <header className="flex items-center h-16 px-4  mb-6">
            <Search />
          </header>
          <main className="grid gap-6 md:grid-cols-3 lg:grid-cols-3 xl:grid-cols-3">
            <MovieCard />
          </main>
        </div>
        {openModal && <MoviesDetailsModal />}
      </QueryClientProvider>
    </>
  );
}
