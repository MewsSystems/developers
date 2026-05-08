import { CircularProgress, Box } from '@mui/material';

const LoadingComponent = () => {
	return (
		<Box
			className='loading'
			display='flex'
			justifyContent='center'
			alignItems='center'
			height='100vh'>
			<CircularProgress />
		</Box>
	);
};

export default LoadingComponent;
