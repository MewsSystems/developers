import "./header.style.css"
import logo from "./assets/logo-movie-app.svg"
import { Link } from "react-router"

export const Header = () => {

    return (
        <header className="header">
            <div className="header__logo">
                <Link to = "/">
                    <img className="header__image" src={logo} alt="movie app logo" />
                </Link>
            </div>
        </header>
    )
}