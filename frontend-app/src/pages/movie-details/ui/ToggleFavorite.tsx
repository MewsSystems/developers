import { Button } from "@chakra-ui/react";
import { useMutationFavoriteMovie } from "@/entities/movie/hooks/useMutationFavorite";
import { useAuth } from "@/entities/auth/api/providers/AuthProvider";
import { FaHeart } from "react-icons/fa";
import { FaRegHeart } from "react-icons/fa";
import { useState } from "react";

export function ToggleFavorite({
  movieId,
  favorite,
}: {
  movieId: number;
  favorite: boolean;
}) {
  const [isFavorite, setIsFavorite] = useState(favorite);
  const auth = useAuth();
  const mudationFavorite = useMutationFavoriteMovie();
  return (
    <Button
      backgroundColor="transparent"
      onClick={async () => {
        const newFavoriteState = !favorite;
        if (auth.accountId && auth.sessionId) {
          mudationFavorite.mutate({
            movieId: movieId,
            favorite: newFavoriteState,
            accountId: auth.accountId,
            sessionId: auth.sessionId,
          });
          setIsFavorite(!isFavorite);
        }
      }}
    >
      {isFavorite ? <FaHeart color="red" /> : <FaRegHeart color="red" />}
    </Button>
  );
}
