import { setUserSearch } from "@/store/movies/slice";
import { useAppDispatch } from "./store";
import { setModalState } from "@/store/modal/slice";
import { setMovieId } from "@/store/MovieId/slice";

export const useMoviesActions = () => {
  const dispatch = useAppDispatch();

  const userSearch = (search: string | undefined) => {
    dispatch(setUserSearch(search));
  };

  return { userSearch };
};

export const useModalState = () => {
  const dispatch = useAppDispatch();

  const setModalStateHandler = (isOpen: boolean) => {
    dispatch(setModalState(isOpen));
  };

  return { setModalState: setModalStateHandler };
};

export const useMovieId = () => {
  const dispatch = useAppDispatch();

  const setMovieIdHandler = (id: number) => {
    dispatch(setMovieId(id));
  };

  return { setMovieId: setMovieIdHandler };
};
