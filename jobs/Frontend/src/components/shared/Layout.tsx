import { Footer, Header, Main, TmdbAttributionText, TmdbLogoImg } from './Layout.styled.tsx';
import tmdbShortLogo from '../../assets/tmdb-short-logo.svg';
import { ReactNode } from 'react';

export default function Layout({ children }: { children: ReactNode }) {
    return (
        <>
            <Header>
                <h1>Mews - Frontend tech task</h1>
            </Header>

            <Main>
                {children}
            </Main>

            <Footer>
                {/* attribution added following TMDBs Terms of Use */}
                <TmdbAttributionText>This website uses TMDB and the TMDB APIs but is not endorsed, certified, or otherwise approved by TMDB.</TmdbAttributionText><br/>
                <TmdbLogoImg alt="TMDB logo" src={tmdbShortLogo} />
            </Footer>
        </>
    );
}
