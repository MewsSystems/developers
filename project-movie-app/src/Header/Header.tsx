import "./header.style.css"
import logo from "./assets/logo-movie-app.svg"

export const Header: React.FC = () => {

    return (
        <header className="header">
            <div className="header__logo">
                <a href = "/">
                    <img className="header__image" src={logo} alt="movie app logo" />
                </a>
            </div>
        </header>
    )
}