import { motion, AnimatePresence } from "framer-motion";
import { Movie, Genres } from './types';
import Placeholder from './assets/Placeholder.png'
import Favicon from './assets/Favicon.svg';

interface MovieDetailsProps
{
	setShowDetails: React.Dispatch<React.SetStateAction<boolean>>;
	selectedMovie: Movie | null;
}

function MovieDetails( {setShowDetails, selectedMovie} : MovieDetailsProps )
{
	return(
		<AnimatePresence>
		<motion.div className="fixed inset-0 backdrop-blur-sm bg-black/50 flex items-center justify-center z-50" initial={{ opacity: 0 }} animate={{ opacity: 1 }} exit={{ opacity: 0 }} onClick={() => setShowDetails(false)}>
			<motion.div className='relative' initial={{ scale: 0.9, y: 20 }} animate={{ scale: 1, y: 0 }} exit={{ scale: 0.9, y: 20 }} transition={{ type: "spring", stiffness: 300, damping: 25 }} onClick={(e) => e.stopPropagation()}>
				<img className="rounded-lg w-[50vw] min-w-lg relative shadow-xl border-4 border-[#ffbd5a]" src={selectedMovie?.backdrop_path ? `https://image.tmdb.org/t/p/w500${selectedMovie?.backdrop_path}` : Placeholder} alt={selectedMovie?.title} />
				<div className="absolute inset-0 bg-gradient-to-t from-black to-transparent opacity-80 rouded-lg"></div>
				<div className="absolute top-2 right-2 bg-[#ffbd5a] text-black px-2 py-1 rounded-full text-xs font-bold shadow-2xl">
                	{selectedMovie?.vote_average.toFixed(1)}
              	</div>
				<div className="absolute flex items-center top-2 left-2 hover:cursor-pointer p-2 rounded-full shadow-2xl" onClick={() => setShowDetails(false)}>
                	<img className="h-4 w-4" src={Favicon}/>
              	</div>

				<div className="absolute flex flex-col bottom-2 left-0 w-full p-5">
                		<h3 className="text-white font-black text-xl truncate">{selectedMovie?.title}</h3>
						<p className='text-m lg:text-lg font-light opacity-70'>{selectedMovie?.overview}</p>
                		<div className="flex justify-between mt-3 items-center overflow-hidden">
                  			<span className="text-gray-300 text-sm transition-opacity duration-300 group-hover:opacity-0">{selectedMovie?.release_date}</span>
                 			{selectedMovie?.genre_ids && selectedMovie?.genre_ids.length > 0 && (<span className="text-xs transition-opacity bg-black/50 py-1 px-2 rounded-2xl duration-300 group-hover:opacity-0">{selectedMovie?.genre_ids.map((genre) => Genres.get(genre)).join(', ')}</span>)}
                		</div>
              		</div>
			</motion.div>
		</motion.div>
		</AnimatePresence>
	)
}

export default MovieDetails