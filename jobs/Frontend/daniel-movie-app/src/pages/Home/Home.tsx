import { useState } from 'react';
import './Home.css';
import { InputText } from 'primereact/inputtext';
import { DataTable } from 'primereact/datatable';
import { Column } from 'primereact/column';
import { useNavigate } from 'react-router-dom';

function Home() {
  // const [searchTerm, setSearchTerm] = useState<string>('');
  const [movies, setMovies] = useState([]);
  const navigate = useNavigate();

  const handleInputChange = (searchTerm: string) => {
    // setSearchTerm(searchTerm);
    let url = 'https://api.themoviedb.org/3/search/movie?query='+searchTerm+'&include_adult=false&language=en-US&page=2&api_key='+process.env.REACT_APP_TMDB_API_KEY;

    fetch(url, {method: 'GET', headers: {accept: 'application/json'}})
      .then(res => {
        if (!res.ok) {
          throw new Error(`HTTP error! Status: ${res.status}`);
        }
        return res.json();
      })
      .then(json => {console.log(json); setMovies(json.results)})
      .catch(err => console.error('Error fetching movies:', err));
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
        onSelectionChange={(e) => navigate('/'+(e.value as any).id)}
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
