import { Button } from "@chakra-ui/react"
import type { DetailsProps } from '@/pages/movie-details/types';
import { useAuth } from "@/entities/auth/api/providers/AuthProvider";
import { FaBookmark } from "react-icons/fa";
import { FaRegBookmark } from "react-icons/fa";

import { useState } from "react";
import { useMutationAddWatchList } from "@/entities/movie/hooks/useMutationWatchList";


export function ToggleWatchList({ detailsProps }: { detailsProps: DetailsProps }) {
    const [isWatchList, setIsWatchList] = useState(detailsProps.movie.account_states.watchlist);
    const auth = useAuth();
    const mudationWatchlist = useMutationAddWatchList();
    return <Button backgroundColor="transparent" onClick={async () => {
        const newWatchListState = !detailsProps.movie.account_states.watchlist
        if (auth.accountId && auth.sessionId) {
            mudationWatchlist.mutate({ movieId: detailsProps.movie.id, watchlist: newWatchListState, accountId: auth.accountId, sessionId: auth.sessionId })
            setIsWatchList(!isWatchList);
        }
    }}>
        {isWatchList ? <FaBookmark color="black" /> : <FaRegBookmark color="black" />}
    </Button>
}
