import { Box, TextField } from '@mui/material';

const SearchBox = ({ query, onSearch }: { query: string; onSearch: (term: string) => void }) => {
	const performSearch = (event: any) => {
		const term = event.target.value;
		// remove white space from search term
		if (term && term.trim().length === 0) {
			return;
		}

		onSearch(term);
	};

	return (
		<Box className='search-box' sx={{ width: 600, maxWidth: '100%' }}>
			<TextField
				label='Search'
				value={query}
				data-test='search'
				onChange={performSearch}
				margin='normal'
				variant='outlined'
				fullWidth
			/>
		</Box>
	);
};

export default SearchBox;
