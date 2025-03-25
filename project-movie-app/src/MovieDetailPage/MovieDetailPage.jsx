import "./movieDetailPage.style.css"
import { Header } from "../Header/Header"
import { Footer } from "../Footer/Footer"
import { Button } from "../Button/Button"

export const MovieDetailPage = () => {

    return (
        <>
            <Header/>
            <main>
                <section className="movie-detail">
                    <h1 className="movie-title">{`The Matrix (1999)`}</h1>
                    <p className="movie-tagline"><em><q>Believe the unbelievable</q></em></p>
                    <div className="movie-content">
                        <div className="movie-poster">
                            <img
                                src={`https://image.tmdb.org/t/p/w500/icmmSD4vTTDKOq2vvdulafOGw93.jpg`}
                                alt="Movie 1"
                            />
                        </div>
                        <div className="movie-info">
                            <p><strong>Genres: </strong>Here are genres</p>
                            <p><strong>User Score:</strong></p>
                            <p><strong>Release Date:</strong></p>
                            <p><strong>Original Language:</strong></p>
                            <p><strong>Runtime:</strong></p>
                            <p><strong>Homepage: </strong>
                                <a href="http://www.warnerbros.com/matrix" target="_blank" rel="noopener noreferrer">
                                    The Matrix
                                </a>
                            </p>
                        </div>
                    </div>
                    <div className="movie-overview">
                        <h2>Movie overview</h2>
                        <p>Set in the 22nd century, The Matrix tells the story of a computer hacker who joins a group of underground insurgents fighting the vast and powerful computers who now rule the earth.</p>
                    </div>
                    <Button label="Back to movies"/>
                </section>
            </main>
            <Footer/>
        </>
    )
}