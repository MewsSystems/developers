import { Modal } from "flowbite-react";
import MoviesDetails from "./MoviesDetails";
import { useModalState } from "@/hooks/useMoviesActions";
import { useAppSelector } from "@/hooks/store";
import { useEffect, useState } from "react";
import { MoviesDetailsFull } from "@/lib/types";
import { useGetMovieClicked } from "@/hooks/useMovies";

export default function MoviesDetailsModal() {
  const movieId = useAppSelector((state) => state.movieId.id);
  const openModal = useAppSelector((state) => state.modalState.isOpen);

  const { setModalState } = useModalState();
  const [movieDetail, setMovieDetails] = useState<MoviesDetailsFull>();
  const { data, isLoading } = useGetMovieClicked(movieId);
  console.log(data);
  useEffect(() => {
    setMovieDetails(data);
  }, [data]);

  return (
    <>
      <Modal show={openModal} size="7xl" onClose={() => setModalState(false)}>
        <Modal.Header className="border-b-o"></Modal.Header>
        <Modal.Body>
          <div className="space-y-6 p-6">
            {isLoading ? (
              <h1> Loading .....</h1>
            ) : (
              <MoviesDetails movieDetails={movieDetail} />
            )}
          </div>
        </Modal.Body>
      </Modal>
    </>
  );
}
