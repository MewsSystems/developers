import { useState, useEffect } from 'react'
import { Movie } from './types'
import axios from 'axios';
import MovieDisplay from './MovieDisplay';
import Pagination from './Pagination';
const API_KEY = import.meta.env.VITE_API_KEY;

interface MovieGridProps
{
	setShowDetails: React.Dispatch<React.SetStateAction<boolean>>;
	setSelectedMovie: React.Dispatch<React.SetStateAction<Movie | null>>;
}

function Moviegrid( {setShowDetails, setSelectedMovie}: MovieGridProps )
{
	const [movies, setMovies] = useState<Movie[]>([]);
	const [showTrending, setShowTrending] = useState(true);
	const [search, setSearch] = useState('');
	const [page, setPage] = useState(1);

	useEffect(() =>
	{
		async function getMovies()
		{
			try
			{
				let response;
				if (search)
					response = await axios.get(`https://api.themoviedb.org/3/search/movie?api_key=${API_KEY}&query=${encodeURIComponent(search)}&page=${page}`)
				else
					response = await axios.get(`https://api.themoviedb.org/3/movie/${showTrending ? 'popular' : 'top_rated'}?api_key=${API_KEY}&page=${showTrending ? '1' : page}`)
				if (response.status === 200)
					setMovies(response.data.results)
				else
					console.error("Error in fetching movies with code: ", response.status);
			}
			catch (error)
			{
				console.error("API error:", error);
			}
		}; getMovies();
	}, [showTrending, page, search]);

	return (
		<div className='flex flex-col justify-start min-h-[calc(100vh-8vh)] gap-5 px-20 py-6'>
			<div className='flex flex-col md:flex-row justify-between items-start md:items-center gap-4 md:gap-0'>
				<div className='flex items-baseline space-x-5'>
					<h2 className={`${showTrending ? 'text-2xl font-bold' : 'text-l font-medium opacity-60 hover:opacity-100 hover:cursor-pointer'}`}  onClick={() => setShowTrending(true)}>Trending Movies</h2>
					<h2 className={`${!showTrending ? 'text-2xl font-bold' : 'text-l font-medium opacity-60 hover:opacity-100 hover:cursor-pointer'}`} onClick={() => setShowTrending(false)} >Top Rated Movies</h2>
				</div>
				<label className="flex items-center gap-2 bg-[#1b1b1b] text-white px-4 py-2 rounded-2xl w-full md:max-w-sm border border-[#323232] focus-within:border-[#ffbd5a] transition">
					<svg className="h-5 w-5 text-gray-400" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"> <circle cx="11" cy="11" r="8" /> <line x1="21" y1="21" x2="16.65" y2="16.65" /> </svg>
					<input className="bg-transparent outline-none w-full text-sm placeholder-gray-400" type="search" placeholder="Search..." onChange={(e) => setSearch(e.target.value)}/>
				</label>
			</div>
			<div className='grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 xl:grid-cols-5 gap-3'>
				{movies?.map((movie, index) => <MovieDisplay movie={movie} index={index} setSelectedMovie={setSelectedMovie} setShowDetails={setShowDetails} />)}
			</div>
			{(!showTrending || search) && movies.length >= 19 && <Pagination page={page} setPage={setPage} />}
		</div>
	)
}

export default Moviegrid