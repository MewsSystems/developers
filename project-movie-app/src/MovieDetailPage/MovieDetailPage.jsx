import { Header } from "../Header/Header"
import { Footer } from "../Footer/Footer"
import { MovieDetail } from "./MovieDetail"

export const MovieDetailPage = () => {

    return (
        <>
            <Header/>
            <main>
                <MovieDetail/>
            </main>
            <Footer/>
        </>
    )
}