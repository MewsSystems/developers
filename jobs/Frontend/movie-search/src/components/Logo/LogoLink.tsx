import { Box, Card, Link, Typography } from '@mui/material';
import LocationSearchingIcon from '@mui/icons-material/LocationSearching';
import { SEARCH_VIEW_ROUTE_PATH } from '../../constants';

const LogoLink = () => {
	return (
		<Box display='flex' justifyContent='center' alignItems={'center'} margin={'10px'}>
			<Link href={SEARCH_VIEW_ROUTE_PATH} sx={{ textDecoration: 'none' }}>
				<Card sx={{ padding: '20px', borderRadius: '80px' }}>
					<Typography variant='h3' component='h3' data-test='heading'>
						M
						<LocationSearchingIcon fontSize='large' sx={{ verticalAlign: 'middle' }} color='info' />
						vie Search
					</Typography>
					<Typography textAlign={'center'} component='div'>
						The world's newest movie website
					</Typography>
				</Card>
			</Link>
		</Box>
	);
};

export default LogoLink;
