import "./header.style.css"
import logo from "./assets/logo-movie-app.svg"

export const Header = () => {

    return (
        <header className="header">
            <div className="header__logo">
                <img className="header__image" src={logo} alt="movie app logo" />
            </div>
        </header>
    )
}