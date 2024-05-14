import { Box, TextField } from '@mui/material';

const SearchBox = ({ query, onSearch }: { query: string; onSearch: (term: string) => void }) => {
	const performSearch = (event: any) => {
		const value = event.target.value;
		if (value && value.trim().length === 0) {
			return;
		}

		onSearch(value);
	};

	return (
		<Box sx={{ width: 600, maxWidth: '100%' }}>
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
