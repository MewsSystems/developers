import { PageSection } from '../components/PageSection';
// import { fetchMovieDetails } from '../search-api';

// console.log(fetchMovieDetails(50));

export const MovieDetail = () => {
  return (
    <>
      <PageSection>
        <img src="" alt="Movie poster" />
        <div>
          <h1>Movie title (release year)</h1>
          <p>genres</p>
        </div>
        <div>
          <p>user rating</p>
        </div>
        <div>
          <h2>Overview</h2>
          <p>overview text</p>
        </div>
      </PageSection>
    </>
  );
};
