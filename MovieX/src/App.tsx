import { useState } from 'react'
import './index.css';
import Navbar from './Navbar';
import MovieDetails from './MovieDetails';
import Moviegrid from './MovieGrid';
import { Movie } from './types'

function App() 
{
	const [showDetails, setShowDetails] = useState(false);
	const [selectedMovie, setSelectedMovie] = useState<Movie | null>(null);

  return (
	<div className='bg-black/70 font-satoshi text-white'>
		<Navbar />
		<Moviegrid setShowDetails={setShowDetails} setSelectedMovie={setSelectedMovie} />
		{showDetails && <MovieDetails setShowDetails={setShowDetails} selectedMovie={selectedMovie}/>}
	</div>
  )
}

export default App