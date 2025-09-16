import { addFavorite } from "@/entities/account/api/addFavorite";
import { useMutation } from "@tanstack/react-query"

export function useMutationFavoriteMovie() {
    return useMutation({
        mutationFn: ({ movieId, favorite, accountId, sessionId }: { movieId: number, favorite: boolean, accountId: number, sessionId: string }) => {
            return addFavorite({ movieId, favorite, accountId, sessionId });
        }
    });
}

