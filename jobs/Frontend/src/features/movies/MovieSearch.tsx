import { useEffect } from "react";
import { useAppDispatch, useAppSelector } from "../../common/hooks";
import { search, selectCurrentPage, selectQuery } from "./moviesSlice";
import Search from "./Search";
import Results from "./Results";

export default function MovieSearch() {
	const query = useAppSelector(selectQuery);
	const page = useAppSelector(selectCurrentPage);
	const dispatch = useAppDispatch();

	useEffect(() => {
		if (query) {
			dispatch(search({ query, page }));
		}
	}, [query, page]);

	return (
		<>
			<Search />
			<Results />
		</>
	);
}
