import { motion } from "framer-motion";
import Placeholder from './assets/Placeholder.png';
import { Movie, Genres } from './types'

interface MovieDisplayProps
{
	movie: Movie;
	index: number;
	setSelectedMovie: React.Dispatch<React.SetStateAction<Movie | null>>;
	setShowDetails: React.Dispatch<React.SetStateAction<boolean>>;
}

function MovieDisplay({movie, index, setSelectedMovie, setShowDetails}: MovieDisplayProps)
{
	return (
		<motion.div key={index} className="relative group hover:cursor-pointer" onClick={() =>{setSelectedMovie(movie); setShowDetails(true)}} whileHover={{scale: 1.02}}>
			<img src={movie.backdrop_path ? `https://image.tmdb.org/t/p/w500${movie.backdrop_path}` : Placeholder} alt={movie.title} />
			<div className="absolute inset-0 bg-gradient-to-t from-black to-transparent opacity-40 group-hover:opacity-100"></div>
		
			<div className="absolute top-2 right-2 bg-[#ffbd5a] text-black px-2 py-1 rounded-full text-xs font-bold shadow-2xl">
				{movie.vote_average.toFixed(1)}
			</div>

			<div className="absolute bottom-0 left-0 w-full p-3">
				<h3 className="text-white font-bold text-lg truncate">{movie.title}</h3>
				<div className="flex justify-between items-center overflow-hidden">
					<span className="text-gray-300 text-sm transition-opacity duration-300 group-hover:opacity-0">{movie.release_date.split('-')[0]}</span>
					{movie.genre_ids && movie.genre_ids.length > 0 && (<span className="text-xs transition-opacity bg-black/50 py-1 px-2 rounded-2xl duration-300 group-hover:opacity-0">{Genres.get(movie.genre_ids[0])}</span>)}
				</div>
			</div>

			<div className='absolute bottom-0 left-0 w-full p-3 opacity-0 transition-opacity duration-300 group-hover:opacity-100'>
				<div className='pr-3 overflow-hidden'>
					<p className='whitespace-nowrap movie-text-hover opacity-70 font-light'>&nbsp;&nbsp;&nbsp;{movie.overview}</p>
				</div>
			</div>
		</motion.div>
	)
}

export default MovieDisplay