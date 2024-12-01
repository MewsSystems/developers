import './MovieSearch.css';
import { useEffect, useState } from 'react';
import { InputText } from 'primereact/inputtext';
import { DataTable } from 'primereact/datatable';
import { Column } from 'primereact/column';
import { useNavigate } from 'react-router-dom';
import { fetchMovies } from '../../services/movieService';
import { Movie } from '../../models/Movie';

const MOVIES_PER_PAGE = 20;

interface LazyTableState {
  first: number | undefined;
  rows: number | undefined;
  page?: number | undefined;
}

function MovieSearch() {
  const navigate = useNavigate();
  const rowsPerPage = MOVIES_PER_PAGE;

  const [searchTerm, setSearchTerm] = useState<string>('');
  const [debouncedSearchTerm, setDebouncedSearchTerm] = useState<string>('');
  const [movies, setMovies] = useState<Array<Movie>>([]);
  const [loading, setLoading] = useState<boolean>(false);
  const [totalRecords, setTotalRecords] = useState<number>(0);
  const [lazyTableState, setLazyTableState] = useState<LazyTableState>({
    first: 0,
    rows: rowsPerPage,
    page: 0,
  });

  useEffect(() => {
    const debounceInput = setTimeout(() => {
      if (searchTerm.trim() !== "") {
        setDebouncedSearchTerm(searchTerm);
        setLazyTableState((prev) => ({ ...prev, first: 0, page: 0 }));
      } else {
        setMovies([]);
        setTotalRecords(0);
      }
    }, 500);

    return () => clearTimeout(debounceInput);
  }, [searchTerm]);

  useEffect(() => {
    setLoading(true);
    fetchMovies(debouncedSearchTerm.trim(), lazyTableState.page ? lazyTableState.page + 1 : 1)
      .then(data => {
        setMovies(data?.results);
        setTotalRecords(data?.total_results);
        setLoading(false);
      })
      .catch(err => {
        console.error('Error fetching movies:', err);
        setLoading(false);
      });
  }, [lazyTableState, debouncedSearchTerm]);

  const handleMovieSelected = (movie: Movie) => {
    navigate('/'+movie.id);
  };

  return (
    <div className='flex flex-column align-items-center'>
      <header>
        <h1 className="text-7xl font-semibold text-center">THE MOVIE APP</h1>
      </header>
      <div className='flex gap-3 align-items-center'>
        <InputText
          value={searchTerm}
          onChange={(e) => setSearchTerm(e.target.value)}
          placeholder="Search movies..."
        />
        <i className='pi pi-search text-2xl text-primary'></i>
      </div>
      <DataTable 
        value={movies} 
        lazy
        paginator
        rows={rowsPerPage} 
        first={lazyTableState.first}
        totalRecords={totalRecords} 
        onPage={(e) =>{setLazyTableState(e)}}
        loading={loading}
        selectionMode="single" 
        onSelectionChange={(e) => handleMovieSelected(e.value)}
        emptyMessage="Nothing found"
        className="mt-5 w-10"
        style={{maxWidth: "80rem"}}
      >
        <Column field="title" header="Title" className='w-8'></Column>
        <Column field="release_date" header="Release date" className='w-2'></Column>
      </DataTable>
    </div>
  );
}

export default MovieSearch;
