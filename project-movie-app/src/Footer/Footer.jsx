import "./footer.style.css"

export const Footer = () => {

    return (
        <footer className="footer">
            © {new Date().getFullYear()} MovieApp — Built with ❤️ by Barbora
        </footer>
    )
}