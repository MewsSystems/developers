import { Outlet } from 'react-router-dom';
import tmdbShortLogo from './assets/tmdb-short-logo.svg';
import { Footer, Header, Main, TmdbAttributionText, TmdbLogoImg } from './App.styled';

export default function App() {
    // TODO: implement movie detail view
    // TODO: add attribution of TMDB
    return (
        <>
            <Header>
                <h1>Mews - Frontend tech task</h1>
            </Header>

            <Main>
                <Outlet />
            </Main>
            
            <Footer>
                {/* attribution added following TMDBs Terms of Use */}
                <TmdbAttributionText>This website uses TMDB and the TMDB APIs but is not endorsed, certified, or otherwise approved by TMDB.</TmdbAttributionText><br/>
                <TmdbLogoImg alt="TMDB logo" src={tmdbShortLogo} />
            </Footer>
        </>
    );
}
