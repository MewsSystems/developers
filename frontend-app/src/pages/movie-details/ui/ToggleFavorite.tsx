import { Button } from "@chakra-ui/react"
import type { DetailsProps } from '@/pages/movie-details/types';
import { useMutationFavoriteMovie } from "@/entities/movie/hooks/useMutationFavorite";
import { useAuth } from "@/entities/auth/api/providers/AuthProvider";
import { FaHeart } from "react-icons/fa";
import { FaRegHeart } from "react-icons/fa";
import { useState } from "react";

export function ToggleFavorite({ detailsProps }: { detailsProps: DetailsProps }) {
    const [isFavorite, setIsFavorite] = useState(detailsProps.movie.account_states.favorite);
    const auth = useAuth();
    const mudationFavorite = useMutationFavoriteMovie();
    return <Button backgroundColor="transparent" onClick={async () => {
        const newFavoriteState = !detailsProps.movie.account_states.favorite
        if (auth.accountId && auth.sessionId) {
            mudationFavorite.mutate({ movieId: detailsProps.movie.id, favorite: newFavoriteState, accountId: auth.accountId, sessionId: auth.sessionId })
            setIsFavorite(!isFavorite);
        }
    }}>
        {isFavorite ? <FaHeart color="red" /> : <FaRegHeart color="red" />}
    </Button>
}

