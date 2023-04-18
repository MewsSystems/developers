import Pagination from "../../components/Pagination";
import { Card, CardContainer } from "../../components/Card";
import { useAppDispatch, useAppSelector } from "../../common/hooks";
import { selectCurrentPage, selectTotalPages, pageChanged, selectResults, selectStatus } from "./moviesSlice";

export default function Results() {
	const results = useAppSelector(selectResults);
	const currentPage = useAppSelector(selectCurrentPage);
	const totalPages = useAppSelector(selectTotalPages);
	const status = useAppSelector(selectStatus);
	const dispatch = useAppDispatch();
	let component;

	if (status === "Loading") {
		component = <p>Loading...</p>;
	}

	if (status === "Failed") {
		component = <p>Search failed.</p>;
	}

	if (status === "Succeeded" && !results.length) {
		component = <p>No results found.</p>;
	}

	if (status === "Succeeded" && results.length) {
		component = (
			<CardContainer>
				{results.map((r) => (
					<Card href={`/movie/${r.id}`} key={r.id} {...r} />
				))}
			</CardContainer>
		);
	}

	return (
		<>
			{component}
			{results.length ? (
				<Pagination
					currentPage={currentPage}
					totalPages={totalPages}
					onPreviousClick={() => dispatch(pageChanged(currentPage - 1))}
					onNextClick={() => dispatch(pageChanged(currentPage + 1))}
				/>
			) : null}
		</>
	);
}
