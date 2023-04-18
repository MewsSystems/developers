import { useCallback } from "react";
import { useAppDispatch } from "../../common/hooks";
import SearchField from "../../components/SearchField";
import { queryChanged } from "./moviesSlice";

export default function Search() {
	const dispatch = useAppDispatch();

	const onSearch = useCallback(
		(query: string) => {
			dispatch(queryChanged(query));
		},
		[dispatch]
	);

	return <SearchField placeholder="Search for a film" onSearch={onSearch} />;
}
