import { Button } from "@chakra-ui/react";
import { useAuth } from "@/entities/auth/api/providers/AuthProvider";
import { FaBookmark } from "react-icons/fa";
import { FaRegBookmark } from "react-icons/fa";

import { useState } from "react";
import { useMutationAddWatchList } from "@/entities/movie/hooks/useMutationWatchList";

export function ToggleWatchList({
  movieId,
  watchlist,
}: {
  movieId: number;
  watchlist: boolean;
}) {
  const [isWatchList, setIsWatchList] = useState(watchlist);
  const auth = useAuth();
  const mudationWatchlist = useMutationAddWatchList();
  return (
    <Button
      backgroundColor="transparent"
      onClick={async () => {
        const newWatchListState = !watchlist;
        if (auth.accountId && auth.sessionId) {
          mudationWatchlist.mutate({
            movieId: movieId,
            watchlist: newWatchListState,
            accountId: auth.accountId,
            sessionId: auth.sessionId,
          });
          setIsWatchList(!isWatchList);
        }
      }}
    >
      {isWatchList ? (
        <FaBookmark color="black" />
      ) : (
        <FaRegBookmark color="black" />
      )}
    </Button>
  );
}
