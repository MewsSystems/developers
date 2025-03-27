import { Header } from "../Header/Header"
import { Footer } from "../Footer/Footer"
import { MovieDetail } from "./MovieDetail"

export const MovieDetailPage: React.FC = () => {

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