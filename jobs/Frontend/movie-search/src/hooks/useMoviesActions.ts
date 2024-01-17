import { setUserSearch } from "@/store/movies/slice";
import { useAppDispatch } from "./store";

export const useMoviesActions = () => {
  const dispatch = useAppDispatch();

  const userSearch = (search: string | undefined) => {
    dispatch(setUserSearch(search));
  };

  return { userSearch };
};
