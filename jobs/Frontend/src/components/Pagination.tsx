import styled from "styled-components";
import { Tile } from "./Tile";
import Button from "./Button";

const Wrapper = styled.div`
	display: flex;
	gap: 0.5rem;
	justify-content: center;
`;

type PaginationProps = {
	onPreviousClick: () => void;
	onNextClick: () => void;
	currentPage: number;
	totalPages: number;
};
export default function Pagination({ onPreviousClick, onNextClick, currentPage, totalPages }: PaginationProps) {
	return (
		<Wrapper>
			<Button disabled={currentPage <= 1} variant="Back" onClick={onPreviousClick} text="Previous" />
			<Tile title={`${currentPage} of ${totalPages}`} />
			<Button disabled={currentPage === totalPages} variant="Forward" onClick={onNextClick} text="Next" />
		</Wrapper>
	);
}
