import { useState } from 'react';
import './Home.css';
import { InputText } from 'primereact/inputtext';
import { DataTable } from 'primereact/datatable';
import { Column } from 'primereact/column';
import { useNavigate } from 'react-router-dom';
import { fetchMovies } from '../../services/movieService';
import { Movie } from '../../models/Movie';

function Home() {
  // const [searchTerm, setSearchTerm] = useState<string>('');
  const [movies, setMovies] = useState<Array<Movie>>([]);
  const navigate = useNavigate();

  const handleInputChange = (searchTerm: string) => {
    // setSearchTerm(searchTerm);
    fetchMovies(searchTerm)
      .then(json => {
        console.log(json); 
        setMovies(json?.results);
      })
      .catch(err => console.error('Error fetching movies:', err));
  };

  const handleMovieSelected = (movie: Movie) => {
    navigate('/'+movie.id, { state: { movie } })
  };

  return (
    <div className='flex flex-column align-items-center'>
      <header>
        <h1 className="text-7xl font-semibold">THE MOVIE APP</h1>
      </header>
      <div className='flex gap-3 align-items-center'>
        <InputText /*value={searchTerm}*/ onChange={(e) => handleInputChange(e.target.value)} />
        <i className='pi pi-search text-2xl text-primary'></i>
      </div>
      <DataTable 
        value={movies} 
        paginator rows={10} 
        selectionMode="single" 
        onSelectionChange={(e) => handleMovieSelected(e.value)}
        tableStyle={{ minWidth: '50rem' }} 
        className='mt-5' 
      >
        <Column field="title" header="Title"></Column>
        <Column field="release_date" header="Release date" className='w-3'></Column>
      </DataTable>
    </div>
  );
}

export default Home;
