import "./footer.style.css"

export const Footer: React.FC = () => {

    return (
        <footer className="footer">
            © {new Date().getFullYear()} MovieApp — Built with ❤️ by Barbora
        </footer>
    )
}