/*
 |-----------------------------------------------------------------------------
 | services/movie/api.ts
 | v1.0.0
 |-----------------------------------------------------------------------------
 */

// const getMovie = async (id: string) => {
// 	try {
// 		if (error && status !== 406) {
// 			console.error(`Error: ${error.message}`);
// 			return null;
// 		}
//
// 		if (data) {
// 			return data;
// 		}
// 	} catch (error) {
// 		console.error(`Error: ${error}`);
// 	}
// };

const getAllMovies = async () => {
	try {
		const response = await fetch(
			`${process.env.NEXT_PUBLIC_API_URL}/movie/now_playing`,
			{
				method: 'GET',
				headers: {
					'Content-Type': 'application/json',
					Authorization: `Bearer ${process.env.NEXT_PUBLIC_API_KEY}`,
				},
			},
		);

		console.log('Debug: ', response);

		if (response.status === 401) {
			console.error(
				`Error: ${response.statusText || 'Error authenticating user'}`,
			);
			return;
		}

		const data = await response.json();

		if (data) {
			return data;
		}
	} catch (error) {
		console.error(`Error: ${error}`);
	}
};

export { getAllMovies };
