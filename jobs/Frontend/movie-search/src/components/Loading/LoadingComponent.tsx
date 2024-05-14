import { CircularProgress, Box } from '@mui/material';

const LoadingComponent = () => {
	return (
		<Box display='flex' justifyContent='center' alignItems='center' height='100vh'>
			<CircularProgress />
		</Box>
	);
};

export default LoadingComponent;
