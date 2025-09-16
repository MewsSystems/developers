import { addWatchList } from "@/entities/account/api/addWatchList";
import { useMutation } from "@tanstack/react-query"

export function useMutationAddWatchList() {
    return useMutation({
        mutationFn: ({ movieId, watchlist, accountId, sessionId }: { movieId: number, watchlist: boolean, accountId: number, sessionId: string }) => {
            return addWatchList({ movieId, watchlist, accountId, sessionId });
        }
    });
}

